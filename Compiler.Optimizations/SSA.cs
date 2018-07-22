using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.ThreeAddrCode.Nodes;
using Compiler.ThreeAddrCode.CFG;
using Compiler.ThreeAddrCode.DFA;
using Compiler.ThreeAddrCode.Expressions;

namespace Compiler.Optimizations
{
    /// <summary>
    /// Класс, обеспечивающий потсроение SSA-формы
    /// </summary>
    public class SSA
    {
        /// <summary>
        /// Исходный код программы
        /// </summary>
        private ThreeAddrCode.TACode Code { get; set; }
        /// <summary>
        /// Граф потока управления
        /// Необходим для промежуточных операций при построении формы
        /// </summary>
        private ControlFlowGraph CFG { get; set; }

        public SSA(ThreeAddrCode.TACode code)
        {
            Code = code;
            CFG = new ControlFlowGraph(code);
        }

        /// <summary>
        /// Операция построения формы. Возвращает преобразованный исходный код программы
        /// </summary>
        public ThreeAddrCode.TACode Apply()
        {
            // Добавляем операторы Фи в начала некоторых блоков
            // Здесь используется информация о достигающий определениях, что не совсем как в презентации, но такой подход выглядит логичнее и оптимальнее
            var reachingDefs = Analyze();
            InjectPhi(reachingDefs);

            // Вычисляем достигающие определений и на их основе строим список агрументов Фи
            CFG = new ControlFlowGraph(CFG.Code);
            reachingDefs = Analyze();
            RelaxPhi(reachingDefs);

            // Переименовываем переменные, захваченные Фи
            CFG = new ControlFlowGraph(CFG.Code);
            RenamePhiOccasions();

            // Удаляем откровенно ненужные Фи (не обязательно, но так красивее и правильнее)
            CFG = new ControlFlowGraph(CFG.Code);
            ReducePhi();

            // Возвращаем преобразованный код
            return CFG.Code;
        }

        /// <summary>
        /// Получить достигающие определения поблочно
        /// </summary>
        /// <returns>Возвращемое значение в формате словаря Базовый блок => пара хеш-сетов - входящие(Item1) и исходящие(Item2) определения для блока</returns>
        private InOutData<HashSet<Guid>> Analyze()
        {
            // Код скопирован из MainForm из вызова опреатора обработки взведенной галочки итерационного алгоритма
            // Есть надежда, что он рабочий
            var op = new ThreeAddrCode.DFA.ReachingDefinitions.Operations(CFG.Code);
            var tf = new ThreeAddrCode.DFA.ReachingDefinitions.TransferFunction(CFG.Code);
            var reachingDefs = new GenericIterativeAlgorithm<HashSet<Guid>>()
            {
                Finish = (a, b) =>
                {
                    var (a1, a2) = a;
                    var (b1, b2) = b;

                    return !a2.Except(b2).Any();
                },
                Comparer = (x, y) => !x.Except(y).Any(),
                Fill = () => (op.Lower, op.Lower)
            };
            var data = reachingDefs.Analyze(CFG,
                new ThreeAddrCode.DFA.ReachingDefinitions.Operations(CFG.Code),
                new ThreeAddrCode.DFA.ReachingDefinitions.TransferFunction(CFG.Code));

            // Заплатка
            // Некоторые определения попадают в список достигающих несмотря на то, что были переопределены в том же блоке
            // Например a = 5; a = 10;. Оба будут считаться достигающими для следующих блоков
            return Verify(data);
        }

        /// <summary>
        /// Очистка ложных достигающих определений
        /// </summary>
        private InOutData<HashSet<Guid>> Verify(InOutData<HashSet<Guid>> reachingDefs)
        {
            // Черный список определений. Определения из этого списка удаляются в каждом входящем(Item1) сете. 
            // Можно и в исходящих, но у меня не было такой потребности
            var defToRemove = new List<Guid>();
            // Перебираем все базовые блоки
            foreach (var block in CFG.CFGNodes)
            {
                // Вычисляем определения, порожденные родительскими ББлоками
                // Для этого из множества исходящих  из них определений вычитаются множества входящих
                // Затем группируются по определяемой переменной
                var parentOwnedDefs = block.Parents.Select(p => reachingDefs[p].Item2.Except(reachingDefs[p].Item1).GroupBy(id => (CFG.Code.LabeledCode[id] as Assign).Result));

                // Правильным определением считается последнее в списке
                // Остальные добавляются в черный список
                var correctDefs = parentOwnedDefs.SelectMany(p => p.Select(gr => gr.LastOrDefault()));
                defToRemove.AddRange(parentOwnedDefs.SelectMany(p => p.SelectMany(gr => gr)).Except(correctDefs));

                // Чистим список входящих определений текущего блока
                reachingDefs[block].Item1.RemoveWhere(id => defToRemove.Contains(id));
            }

            return reachingDefs;
        }

        /// <summary>
        /// Вставляем операторы Фи, используя данные о достигающих переменных
        /// </summary>
        private void InjectPhi(InOutData<HashSet<Guid>> reachingDefs)
        {
            // Перебираем блоки
            foreach (var block in reachingDefs.Keys)
            {
                // Если у блока один или ноль предков, пропускае его (не точка сбора)
                if (block.Parents.Count() < 2)
                    continue;

                // Если в блок не приходят достигающие определения извне, пропускаем его
                // Добавлять Фи в общем-то бестолку
                var innerDefs = reachingDefs[block].Item1.ToList();
                if (innerDefs.Count == 0)
                    continue;

                // Группируем достигающие определения по определяемой переменной
                var groupedDefs = innerDefs.Select(def => CFG.Code.LabeledCode[def] as Assign).GroupBy(ass => ass.Result);

                // Перебираем переменные
                foreach (var gr in groupedDefs)
                {
                    // Если для переменной достигается более одного опрделения
                    if ((gr.Count() > 1) && IsInjectionNecessary(block, gr.Key))
                    {
                        // Добавляем оператор Фи с пустым списком аргументов (он потом будет пересчитан в Relax)
                        var phi = new Phi()
                        {
                            Result = gr.Key,
                            Left = null,
                            Right = gr.Key,
                            Operation = ThreeAddrCode.OpCode.Phi,
                            DefenitionList = null
                        };

                        // Поскольку Фи добавляется в начало блока, есть опасность нарушить целостность блока
                        // Если Фи добавляется перед помеченым оператором, то метка переносится на Фи
                        var firstOp = block.CodeList.FirstOrDefault();
                        if (firstOp != null)
                        {
                            CFG.Code.InsertNode(phi, firstOp.Label);
                            if (firstOp.IsLabeled)
                                CFG.Code.MoveLabel(firstOp, phi);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Вычисляем аргументы Фи, используя информацию о достигающих определениях ( в т.ч. других Фи)
        /// </summary>
        /// <param name="reachingDefs"></param>
        private void RelaxPhi(InOutData<HashSet<Guid>> reachingDefs)
        {
            // Перебираем базовые блоки
            foreach (var block in reachingDefs.Keys)
            {
                // Пропускаем блоки, в которых точно нет Фи
                if (block.Parents.Count() < 2)
                    continue;

                var innerDefs = reachingDefs[block].Item1.ToList();
                if (innerDefs.Count == 0)
                    continue;

                // Группируем входящие определения текущего блока по определяемой переменной
                var groupedDefs = innerDefs.Select(def => CFG.Code.LabeledCode[def] as Assign).GroupBy(ass => ass.Result);

                // Перебираем переменные
                foreach (var gr in groupedDefs)
                {
                    // В блоке ищем оператор Фи, определяющий текущую переменную
                    var phiNode = block.CodeList.ToList().Find(node => (node is Phi phi) && (phi.Result == gr.Key)) as Phi;
                    // Если такой обнаружен
                    if (phiNode != null)
                    {
                        // Определяем его список аргументов как все достигающие определения этой переменной
                        phiNode.DefenitionList = gr.ToList();
                        foreach (var ass in phiNode.DefenitionList)
                        {
                            // Для каждого достигающего определения сохраняем дугу CFG, по которой оно передается
                            // Для этого среди предков текущего блока находим такой, у которого данное определение есть в списке исходящих
                            var bb = block.Parents.ToList().Find(p => reachingDefs[p].Item2.Contains(ass.Label));
                            phiNode.DefinitionPathes.Add(ass, bb);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Переименовать занятые Фи переменные
        /// </summary>
        private void RenamePhiOccasions()
        {
            // Создаем счетчик для каждой переменной
            var counter = CFG.Code.CodeList.Where(node => node is Phi).Cast<Phi>().GroupBy(phi => phi.Result).ToDictionary(gr => gr.Key, gr => 1);

            // Запускаем обход в глубину графа потока управления
            dfs_visit(CFG.GetRoot(), new List<Guid>(), new Dictionary<Var, Var>(), counter);
        }
                      
    
        /// <summary>
        /// Обход CFG в глубину
        /// </summary>
        /// <param name="curBlock">Текущий базовый блок</param>
        /// <param name="usedBlocks">Список посещенных блоков</param>
        /// <param name="varSubstitution">Словарь подстановок. Здесь хранится исходное имя переменной и переменная с индексом, которая является текущей</param>
        /// <param name="counter">Словарь индексов. Хранит информацию о следующем по порядку индексе для каждой переменной</param>
        private void dfs_visit(BasicBlock curBlock, List<Guid> usedBlocks, Dictionary<Var, Var> varSubstitution, Dictionary<Var, int> counter)
        {
            // Помечаем ББлок, в который пришли, использованным
            usedBlocks.Add(curBlock.BlockId);

            // Перебираем операторы блока
            foreach (var node in curBlock.CodeList)
                switch (node)
                {
                    // Если попался Фи, то к его результату прибавляется следующий по порядку индекс и эта проиндексированная переменная становится текущей
                    case Phi phi:
                        var old = phi.Result;
                        phi.Result = new Var($"{phi.Result}_{counter[phi.Result]++}");
                        if (varSubstitution.ContainsKey(old))
                            varSubstitution[old] = phi.Result;
                        else
                            varSubstitution.Add(old, phi.Result);
                        break;

                    // Если попался оператор присваивания
                    case Assign ass:
                        // Если один из его операндов является переменной, подлежащей переименования, заменяем его на текущую согласно словарю
                        if (ass.Left is Var vL && varSubstitution.ContainsKey(vL))
                            ass.Left = varSubstitution[vL];
                        if (ass.Right is Var vR && varSubstitution.ContainsKey(vR))
                            ass.Right = varSubstitution[vR];
                        // Если переименованию подлежит результат, то прибавляется следующий по порядку индекс и эта проиндексированная переменная становится текущей
                        if (varSubstitution.ContainsKey(ass.Result))
                        {
                            old = ass.Result;
                            ass.Result = new Var($"{ass.Result}_{counter[ass.Result]++}");
                            varSubstitution[old] = ass.Result;
                        }
                        break;

                    // Если попался условный оператор
                    case IfGoto ifgoto:
                        // Если его условие является переменной, подлежащей переименования, заменяем его на текущую согласно словарю
                        if (ifgoto.Condition is Var vC && varSubstitution.ContainsKey(vC))
                            ifgoto.Condition = varSubstitution[vC];
                        break;

                    // Если попался оператор печати
                    case Print print:
                        // Если его операнд является переменной, подлежащей переименования, заменяем его на текущую согласно словарю
                        if (print.Data is Var v && varSubstitution.ContainsKey(v))
                            print.Data = varSubstitution[v];
                        break;                    
                }

            // Перебираем потомков текущего блока, и если их нет в списке использованных, входим в них
            // varSubstitution.ToDictionary(...) используется для разрыва ссылочной целостности. Иначе информация о текущей переменной поднимется из глубины dfs, а не передастся из текущего блока
            foreach (var block in curBlock.Children)
                if (!usedBlocks.Contains(block.BlockId))
                    dfs_visit(block, usedBlocks, varSubstitution.ToDictionary(pair => pair.Key, pair => pair.Value), counter);
        }

        /// <summary>
        /// Элементарное отсечение Фи
        /// </summary>
        private void ReducePhi()
        {
            // Перебираем операторы (два foreach обусловлены необходимостью менять коллекцию оператором Replace)
            foreach (var block in CFG.CFGNodes)
            foreach (var node in block.CodeList)
            {
                // Если это оператор Фи с единственным аргументом
                if ((node is Phi phiNode) && (phiNode.DefenitionList.Count == 1))
                {
                    // Заменяем его на оператор присваивания типа копирование с тем же аргументом
                    var ass = new Assign()
                    {
                        Result = phiNode.Result,
                        Left = null,
                        Right = (phiNode.DefenitionList[0] as Assign).Result,
                        Operation = ThreeAddrCode.OpCode.Copy
                    };

                    CFG.Code.ReplaceNode(ass, phiNode);
                }
            }
        }
        
        /// <summary>
        /// Служебная функция
        /// Проверяет, есть ли использование переменной в блоке до ее переопределения
        /// </summary>
        private bool IsInjectionNecessary(BasicBlock block, Var v)
        {
            foreach (var node in block.CodeList)
                switch (node)
                {
                    case Assign ass:
                        if (ass.Left == v)
                            return true;
                        if (ass.Right == v)
                            return true;
                        if (ass.Result == v)
                            return false;
                        break;

                    case IfGoto ifgoto:
                        if (ifgoto.Condition == v)
                            return true;
                        break;

                    case Print print:
                        if (print.Data == v)
                            return true;
                        break;
                }

            return true;
        }

        /// <summary>
        /// Вставка Фи строго согласно презентации
        /// Работает плохо и неправильно
        /// Просто для справки
        /// </summary>
        private void InjectPhi()
        {
            // Перебираем базовые блоки
            foreach (var block in CFG.CFGNodes)
            {
                // Пропускаем те, у которых ноль или один предок
                if (block.Parents.Count() < 2)
                    continue;

                // Получаем список переменных, упомянутых в блоке
                var varList = new Dictionary<Guid, Var>();
                foreach (var node in block.CodeList)
                    switch (node)
                    {
                        case Assign ass:
                            if (ass.Left is Var vL && !varList.ContainsKey(vL.Id))
                                varList.Add(vL.Id, vL);
                            if (ass.Right is Var vR && !varList.ContainsKey(vR.Id))
                                varList.Add(vR.Id, vR);
                            if (ass.Result is Var res && !varList.ContainsKey(res.Id))
                                varList.Add(res.Id, res);
                            break;

                        case IfGoto ifgoto:
                            if (ifgoto.Condition is Var vC && !varList.ContainsKey(vC.Id))
                                varList.Add(vC.Id, vC);
                            break;

                        case Print print:
                            if (print.Data is Var v && !varList.ContainsKey(v.Id))
                                varList.Add(v.Id, v);
                            break;
                    }

                // Для каждой такой переменной добавляем оператор Фи
                foreach (var v in varList.Values)
                {
                    var phi = new Phi()
                    {
                        Result = v,
                        Left = null,
                        Right = v,
                        Operation = ThreeAddrCode.OpCode.Phi
                    };
                    var firstOp = block.CodeList.FirstOrDefault();
                    if (firstOp != null)
                    {
                        CFG.Code.InsertNode(phi, firstOp.Label);
                        if (firstOp.IsLabeled)
                            CFG.Code.MoveLabel(firstOp, phi);
                    }
                }
            }
        }
    }
}
