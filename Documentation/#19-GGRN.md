### Название задачи
Общий алгоритм при наличии вектора оптимизаций (О1, О2, ...)

#### Постановка задачи
Необходимо реализовать алгоритм, который применял бы все возможные оптимизации для базового блока.

#### Зависимости задач в графе задач
Зависит от:
- Все реализованные оптимизации для базового блока

#### Теоретическая часть задачи
Требуется реализовать алгоритм, который применил бы к базовому блоку все имеющиеся оптимизации до тех пор, пока это возможно делать.

#### Практическая часть задачи (реализация)
Был реализован класс `AllOptimizations` и функция `ApplyAllOptimizations`, применяющая все оптимизации к трехадресному коду.
```
	public class AllOptimizations
	{
		private List<IOptimization> BasicBlockOptimizationList()
		{
			List<IOptimization> optimizations = new List<IOptimization>();

			optimizations.Add(new CopyPropagation());
			optimizations.Add(new ConstantFolding());
			optimizations.Add(new ConstantPropagation());
			optimizations.Add(new DeclarationOptimization());
			optimizations.Add(new AlgebraicOptimization());
			optimizations.Add(new SubexpressionOptimization());

			return optimizations;
		}

        private List<IOptimization> O2OptimizationList()
        {
            return new List<IOptimization>();
        }


        public TACode ApplyAllOptimizations(TACode code)
		{
			List<IOptimization> o1Optimizations = BasicBlockOptimizationList();
            var canApplyAny = true;

            while (canApplyAny)
            {
                canApplyAny = false;
                var blocks = code.CreateBasicBlockList().ToList();
                var codeList = new List<Node>();

                foreach (var b in blocks)
                {
                    var block = b.CodeList.ToList();
                    for (int i = 0; i < o1Optimizations.Count; i++)
                    {
                        block = o1Optimizations[i].Optimize(block, out var applied);
                        canApplyAny = canApplyAny || applied;
                    }
                    codeList.AddRange(block);
                }

                code = new TACode();
                code.CodeList = codeList;


                foreach (var line in code.CodeList)
                    code.LabeledCode[line.Label] = line;
            }

			return code;
		}
	}

```
#### Тесты
```
	a = b
	c = b - a   -----> c = 0
	n = 20
	c = 20 * 3  -----> c = 60
	d = 10 + n  -----> d = 30
```
#### Пример работы.
```
		public void Test1()
		{
			var taCodeAllOptimizations = new TACode();
			var assgn1 = new Assign()
			{
				Left = null,
				Operation = OpCode.Copy,
				Right = new Var(),
				Result = new Var()
			};
			var assgn2 = new Assign()
			{
				Left = assgn1.Right,
				Operation = OpCode.Minus,
				Right = assgn1.Result,
				Result = new Var()
			};
			var assgn3 = new Assign()
			{
				Left = null,
				Operation = OpCode.Copy,
				Right = new IntConst(20),
				Result = new Var()
			};
			var assgn4 = new Assign()
			{
				Left = new IntConst(20),
				Operation = OpCode.Mul,
				Right = new IntConst(3),
				Result = new Var()
			};
			var assgn5 = new Assign()
			{
				Left = new IntConst(10),
				Operation = OpCode.Plus,
				Right = assgn3.Result,
				Result = new Var()
			};
			taCodeAllOptimizations.AddNode(assgn1);
			taCodeAllOptimizations.AddNode(assgn2);
			taCodeAllOptimizations.AddNode(assgn3);
			taCodeAllOptimizations.AddNode(assgn4);
			taCodeAllOptimizations.AddNode(assgn5);

			var allOptimizations = new AllOptimizations();
			allOptimizations.ApplyAllOptimizations(taCodeAllOptimizations);

			Assert.AreEqual(assgn2.Right, 0);
			Assert.AreEqual(assgn4.Right, 60);
			Assert.AreEqual(assgn5.Right, 30);
			Assert.True(true);
		}
```      
