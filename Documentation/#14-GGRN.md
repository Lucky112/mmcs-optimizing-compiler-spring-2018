### Название задачи
Свертка констант, протяжка констант

#### Постановка задачи
Создать класс, реализующий итерационный алгоритм для задачи распостронения констант, и класс, реализующий свертку констант.

#### Зависимости задач в графе задач
Зависит от:
- Трехадресный код

От задачи зависит:
- Общий алгоритм при наличии вектора оптимизаций (О1, О2, ...)

#### Теоретическая часть задачи
Распространение констант – хорошо известная проблема глобального анализа потока данных. Цель распространения констант состоит в обнаружении величин, которые являются постоянными при любом возможном пути выполнения программы, и в распространении этих величин так далеко по тексту программы, как только это возможно. Выражения, чьи операнды являются константами, могут быть вычислены на этапе компиляции. Поэтому использование алгоритмов распространения констант позволяет компилятору выдавать более компактный и быстрый код.

Рассмотрим следующий пример:
```
int x = 14;
int y = 7 - x / 2;
return y * (28 / x + 2);
```

Распространение x возвращает:
```
int x = 14;
int y = 7 - 14 / 2;
return y * (28 / 14 + 2);
```

Далее, свёртка констант и распространение y возвращают следующее:
```
int x = 14;
int y = 0;
return 0;
```

#### Практическая часть задачи (реализация)
Были реализованы классы `ConstantFolding` и `ConstantPropagation`.
```
public class ConstantFolding : IOptimization
	{
		private bool SetNode(Assign node)
		{
			switch (node.Operation)
			{
				case OpCode.Plus:
					node.Right = (IntConst)node.Left + (IntConst)node.Right;
					node.Operation = OpCode.Copy;
					node.Left = null;
					break;
				case OpCode.Minus:
					node.Right = (IntConst)node.Left - (IntConst)node.Right;
					node.Operation = OpCode.Copy;
					node.Left = null;
					break;
				case OpCode.Mul:
					node.Right = (IntConst)node.Left * (IntConst)node.Right;
					node.Operation = OpCode.Copy;
					node.Left = null;
					break;
				case OpCode.Div:
					node.Right = (IntConst)node.Left / (IntConst)node.Right;
					node.Operation = OpCode.Copy;
					node.Left = null;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			return true;
		}
		public List<Node> Optimize(List<Node> nodes, out bool applied)
		{
			var app = false;
			var enumerable = nodes.OfType<Assign>().Where(assgn => assgn.Left is IntConst && assgn.Right is IntConst);

			foreach (var node in enumerable)
				app = SetNode(node);

			applied = app;
			return nodes;
		}
	}

```
```
public class ConstantPropagation : IOptimization
	{
		public List<Node> Optimize(List<Node> nodes, out bool applied)
		{
			var app = false;
			for (int i = 0; i < nodes.Count; i++)
			{
				if (nodes[i] is Assign node && node.Operation == OpCode.Copy && node.Right is IntConst)
				{
					for (int j = i + 1; j < nodes.Count; j++)
					{
						if (nodes[j] is Assign nextNode)
						{
							//Если мы встретили объявление этого же элемента
							if (node.Result.Equals(nextNode.Result))
								break;
							//Проверка использования Result в левом операнде другого узла
							if (node.Result.Equals(nextNode.Left))
							{
								nextNode.Left = node.Right;
								nodes[j] = nextNode;
								app = true;
							}
							//Проверка использования Result в правом операнде другого узла
							if (node.Result.Equals(nextNode.Right))
							{
								nextNode.Right = node.Right;
								nodes[j] = nextNode;
								app = true;
							}
						}
                        if (nodes[j] is Print printNode)
						{
							//Если Result равна тому, что находится в Print и левый операнд node пустой
							if (node.Left is null && node.Result.Equals(printNode.Data))
							{
								printNode.Data = node.Right;
								app = true;
							}
						}
					}
				}
			}
			applied = app;
			return nodes;
		}
	}
```
#### Тесты
Свёртка констант:
```
	a = b
	c = 20 * 3    -----> c = 60
	d = 10 + 1    -----> d = 11
	e = 100 / 50  -----> e = 2
	a = 30 - 20   -----> a = 10
	k = c + a
```
Протяжка констант:
```
	a = 10
	c = b - a     -----> c = b - 10
	d = c + 1
	e = d * a     -----> e = d * 10
	a = 30 - 20
	k = c + a     -----> k = c + a
```
#### Пример работы.
Свёртка констант:
```
	public void Test1()
		{
			var taCodeConstantFolding = new TACode();
			var assgn1 = new Assign()
			{
				Left = null,
				Operation = OpCode.Copy,
				Right = new Var(),
				Result = new Var()
			};
			var assgn2 = new Assign()
			{
				Left = new IntConst(20),
				Operation = OpCode.Mul,
				Right = new IntConst(3),
				Result = new Var()
			};
			var assgn3 = new Assign()
			{
				Left = new IntConst(10),
				Operation = OpCode.Plus,
				Right = new IntConst(1),
				Result = new Var()
			};
			var assgn4 = new Assign()
			{
				Left = new IntConst(100),
				Operation = OpCode.Div,
				Right = new IntConst(50),
				Result = new Var()
			};
			var assgn5 = new Assign()
			{
				Left = new IntConst(30),
				Operation = OpCode.Minus,
				Right = new IntConst(20),
				Result = assgn1.Result
			};
			var assgn6 = new Assign()
			{
				Left = assgn2.Result,
				Operation = OpCode.Plus,
				Right = assgn5.Result,
				Result = new Var()
			};

			taCodeConstantFolding.AddNode(assgn1);
			taCodeConstantFolding.AddNode(assgn2);
			taCodeConstantFolding.AddNode(assgn3);
			taCodeConstantFolding.AddNode(assgn4);
			taCodeConstantFolding.AddNode(assgn5);
			taCodeConstantFolding.AddNode(assgn6);

			var optConstFold = new ConstantFolding();
			optConstFold.Optimize(taCodeConstantFolding.CodeList.ToList(), out var applConstFold);

			Assert.AreEqual(assgn2.Right, 60);
			Assert.AreEqual(assgn3.Right, 11);
			Assert.AreEqual(assgn4.Right, 2);
			Assert.AreEqual(assgn5.Right, 10);
			Assert.True(true);
		}
```
Протяжка констант:
```
public void Test1()
		{
			var taCodeConstProp = new TACode();
			var assgn1 = new Assign()
			{
				Left = null,
				Operation = OpCode.Copy,
				Right = new IntConst(10),
				Result = new Var()
			};
			var assgn2 = new Assign()
			{
				Left = new Var(),
				Operation = OpCode.Minus,
				Right = assgn1.Result,
				Result = new Var()
			};
			var assgn3 = new Assign()
			{
				Left = assgn2.Result,
				Operation = OpCode.Plus,
				Right = new IntConst(1),
				Result = new Var()
			};
			var assgn4 = new Assign()
			{
				Left = assgn3.Result,
				Operation = OpCode.Mul,
				Right = assgn1.Result,
				Result = new Var()
			};
			var assgn5 = new Assign()
			{
				Left = new IntConst(30),
				Operation = OpCode.Minus,
				Right = new IntConst(20),
				Result = assgn1.Result
			};
			var assgn6 = new Assign()
			{
				Left = assgn2.Result,
				Operation = OpCode.Plus,
				Right = assgn5.Result,
				Result = new Var()
			};

			taCodeConstProp.AddNode(assgn1);
			taCodeConstProp.AddNode(assgn2);
			taCodeConstProp.AddNode(assgn3);
			taCodeConstProp.AddNode(assgn4);
			taCodeConstProp.AddNode(assgn5);
			taCodeConstProp.AddNode(assgn6);

			var optConstProp = new CopyPropagation();
			optConstProp.Optimize(taCodeConstProp.CodeList.ToList(), out var applCopProp);

			Assert.AreEqual(assgn2.Right, assgn1.Result);
			Assert.AreEqual(assgn4.Right, assgn1.Result);
			Assert.AreNotSame(assgn6.Right, assgn1.Result);
			Assert.True(true);
		}
```