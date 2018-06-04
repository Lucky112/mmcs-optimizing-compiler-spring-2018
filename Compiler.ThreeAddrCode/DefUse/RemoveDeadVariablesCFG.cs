using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.ThreeAddrCode.CFG;
using Compiler.ThreeAddrCode.Nodes;
using Compiler.ThreeAddrCode.Expressions;

namespace Compiler.ThreeAddrCode
{
    // Тип для множества активных переменных
    using ActiveVar = HashSet<Var>;
    // Список мертвых переменных
    using DVars = List<DUVar>;
    // Список живых переменных
    using LVars = List<DUVar>;

    /// <summary>
    /// Класс для удаления мертвого кода в CFG 
    /// </summary>
    public class RemoveDeadVariablesCFG
    {
        /// <summary>
        /// Граф исходной программы
        /// </summary>
        public TACode CodeIN { get; }
        /// <summary>
        /// Граф программы без мертвого кода
        /// </summary>
        public TACode  CodeNew { get; }

        /// <summary>
        /// Список мертвых переменных
        /// </summary>
        private List<Guid> removeVars;
        /// <summary>
        /// Список активных переменных на выходе
        /// для каждого блока в CFG
        /// </summary>
        private Dictionary<Guid, ActiveVar> OUT;

        /// <summary>
        /// Класс для удаления мертвого кода 
        /// в CFG
        /// </summary>
        /// <param name="CFG"></param>
        public RemoveDeadVariablesCFG(TACode code)
        {
            this.CodeIN = code;
            this.removeVars = new List<Guid>();
            this.CodeNew = RemoveDeadCodeInCFG();
        }

        /// <summary>
        /// Удаление мертвого кода в CFG
        /// </summary>
        /// <returns></returns>
        private TACode RemoveDeadCodeInCFG()
        {
            var code = new TACode();
            code.CodeList = CodeIN.CodeList.ToList();
            ControlFlowGraph cfg;
            int countRemove;

            do
            {
                // Вычисляем CFG
                cfg = new ControlFlowGraph(code);
                // Вычисляем IN и OUT переменные для всех блоков в CFG
                this.OUT = (new IterativeAlgorithmAV(cfg)).OUT;
                countRemove = 0;

                // Для каждого блока в cfg
                foreach (var B in cfg.CFGNodes)
                {
                    // Удаляем мертвые строки кода
                    var newB = RemoveDeadCodeInBlock(B);
                    var curCountRem = B.CodeList.Count() - newB.CodeList.Count();

                    if (curCountRem != 0)
                    {
                        var idxStart = CalculateIdxStart(B, code.CodeList);
                        var len = B.CodeList.Count();
                        code = ReplaceCode(code, newB.CodeList.ToList(), idxStart, len);
                        countRemove += curCountRem;
                    }
                }
            }
            while (countRemove != 0);

            return code;
        }

        /// <summary>
        /// Вычисляет индекс в исходном коде
        /// </summary>
        /// <returns></returns>
        private int CalculateIdxStart(BasicBlock Block, List<Node> listNode)
        {
            var label = Block.CodeList.First().Label;
            return listNode.FindIndex(x => x.Label == label);
        }

        /// <summary>
        /// Заменияет код
        /// </summary>
        /// <returns></returns>
        private TACode ReplaceCode(TACode code, List<Node> nodes, int idxStart, int len)
        {
            var newCode = new List<Node>();

            newCode.AddRange(code.CodeList.Take(idxStart));
            newCode.AddRange(nodes);
            newCode.AddRange(code.CodeList.Skip(idxStart + len));

            var TA = new TACode();
            TA.CodeList = newCode;

            return TA;
        }

        /// <summary>
        /// Определение живых/мертвых переменных для участка кода
        /// </summary>
        /// <param name="listNode"></param>
        private void DetectionLiveAndDeadVariables(List<Node> listNode, Guid idx, IEnumerable<BasicBlock> parents)
        {
            // Определение живых мертвых переменных для блока
            var LDV = new LiveAndDeadVariables(listNode);
            var deadVars = LDV.DeadVars;
            var liveVars = LDV.LiveVars;
            ActiveVar vars = OUT[idx];

            // Для каждой out переменной 
            foreach (var v in vars)
            {
                // Если она есть в списке мертвых переменных
                var dVars = deadVars.FindAll(x => x.Name == v);
                if (dVars.Count != 0)
                {
                    // Находим самую последнюю переменную
                    var max = dVars.Max(x => x.StringId);
                    var j = deadVars.FindIndex(x => x.Name == v && x.StringId == max);

                    // Перемещаем переменную в список живых переменных
                    liveVars.Add(new DUVar(deadVars[j].Name, deadVars[j].StringId));
                    deadVars.RemoveAt(j);
                }                
            }

            // Для каждой in переменной
            foreach (var v in LDV.UListNotValid)
            {
                var IsExist = false;

                // Если существует определение переменной в родительском блоке
                foreach (var p in parents)
                    IsExist |= OUT[p.BlockId].Contains(v.Name);

                // Если она есть в списке неопределенных переменных
                if (IsExist)
                    // Добавляем переменнцю в списко живых
                    liveVars.Add(new DUVar(v.Name, v.StringId));
                else
                    // Добавляем переменнцю в списко мертвых
                    deadVars.Add(new DUVar(v.Name, v.StringId));
            }

            foreach (var dV in deadVars)
                removeVars.Add(dV.StringId);

            removeVars = removeVars.Distinct().ToList();
        }

        /// <summary>
        /// Удаление мертвого кода в блоке
        /// </summary>
        private BasicBlock RemoveDeadCodeInBlock(BasicBlock Block)
        {
            var listNode = Block.CodeList.ToList();
            var idx = Block.BlockId;
            var inds = new Dictionary<int, Guid>();

            var i = 0;
            foreach (var node in listNode)
                inds.Add(i++, node.Label);

            // Определяем живые/мертвые переменные
            DetectionLiveAndDeadVariables(listNode, idx, Block.Parents.ToList());

            // Пока мы не удалим все мертвые переменные
            while (removeVars.Count != 0)
            {
                foreach (var rV in removeVars)
                {
                    var ind = listNode.FindIndex(x => x.Label == rV);
                    listNode.RemoveAt(ind);
                }

                removeVars.Clear();

                // Определяем живые/мертвые переменные
                DetectionLiveAndDeadVariables(listNode, idx, Block.Parents.ToList());
            }

            return new BasicBlock(listNode.ToList());
        }
    }
}
