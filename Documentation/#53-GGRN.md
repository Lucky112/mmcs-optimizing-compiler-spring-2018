### Название задачи
Оптимизация "Распространение констант" между базовыми блоками

#### Постановка задачи
Реализовать протяжку констант, но уже между базовыми блоками.

#### Зависимости задач в графе задач
Зависит от:
- Базовые структуры и итерационный алгоритм для распросранения констант

#### Теоретическая часть задачи
Необходимо разрешить оптимизацию распространения констант между базовыми блоками. Для этого нужно получить данные из итерационного алгоритма для распространения констант, а затем правильно их применить для трёхадресного кода, чтобы в итоге код стал оптимизированным.Итерационный алгоритм присваивает переменным одно из трёх значений: Not A Constant, Undefined или IsConstant.

#### Практическая часть задачи (реализация)
```
		public InOutData<Dictionary<Guid, VarValue>> TempFunc(TACode taCode, ControlFlowGraph cfg)
		{
			Operations ops = new Operations(taCode);
			TransferFunction f = new TransferFunction();

			IterativeAlgorithm itAlg = new IterativeAlgorithm();
			var result = itAlg.Analyze(cfg, ops, f);

			return result;
		}
		public TACode Optimize(TACode taCode, out bool applied)
		{
			var app = false;
			var visited = new Dictionary<Guid, bool>();
			ControlFlowGraph cfg = new ControlFlowGraph(taCode);
			var ioData = TempFunc(taCode, cfg);

			foreach (var node in taCode.CodeList.ToList().OfType<Assign>())
				visited[node.Result.Id] = false;

			for (int j = taCode.CodeList.Count() - 1; j > 0; j--)
			{
				var node = taCode.CodeList.ElementAt(j) as Assign;
				if (node != null)
				{
					for (int i = 0; i < cfg.CFGNodes.Count(); i++)
					{
						if (ioData[cfg.CFGNodes.ElementAt(i)].Item1.ContainsKey(node.Result.Id) && ioData[cfg.CFGNodes.ElementAt(i)].Item1[node.Result.Id].varType is VarValue.Type.CONST)
						{
							if (visited[node.Result.Id] == true)
								break;
							visited[node.Result.Id] = true;
							node.Right = ioData[cfg.CFGNodes.ElementAt(i)].Item1[node.Result.Id].value;
							node.Left = null;
							node.Operation = OpCode.Copy;
							taCode.CodeList[j] = node;
						}
					}
				}
			}

			applied = app;
			return taCode;
		}
```
#### Тесты
Например, есть такой фрагмент кода, состоящий из двух базовых блоков:
```
a = 91;
b = 5;

goto h;

h: c = b - 1;
```
После глобальной оптимизации распространения констант должно получится следующее:
```
a = 91;
b = 5;

goto h;

h: c = 5 - 1;
```

#### Пример работы.
Большой пример, демонстрирующий алгоритм.
