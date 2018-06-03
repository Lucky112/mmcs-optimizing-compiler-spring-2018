### Название задачи
Протяжка копий

#### Постановка задачи
Создать класс, реализующий алгоритм протяжки копий.

#### Зависимости задач в графе задач
Зависит от:
- Трехадресный код

От задачи зависит:
- Общий алгоритм при наличии вектора оптимизаций (О1, О2, ...)

#### Теоретическая часть задачи
Протяжка копий – еще одна известная проблема глобального анализа потока данных. Цель протяжки копий – заменить переменные их значениями.

Следующий фрагмент кода
```
y = x;
z = 3 + y 
```
после выполнения протяжки копий будет выглядеть вот так:
```
z = 3 + x
```

#### Практическая часть задачи (реализация)
Был реализован класс `CopyPropagation`.
```
public class CopyPropagation : IOptimization
	{
		public List<Node> Optimize(List<Node> nodes, out bool applied)
		{
			var app = false;
			for (int i = 0; i < nodes.Count; i++)
			{
				if (nodes[i] is Assign node && node.Operation == OpCode.Copy && !(node.Right is IntConst))
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
					}
				}
			}
			applied = app;
			return nodes;
		}
	}
```
#### Тесты
Протяжка копий:
```
	a = b
	c = b - a     -----> c = b - b
	d = c + 1
	e = d * a     -----> e = d * b
	a = 30 - 20
	k = c + a     -----> k = c + a
```
#### Пример работы.
```
public void Test1()
        {
            var taCodeCopyProp = new TACode();
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

            taCodeCopyProp.AddNode(assgn1);
            taCodeCopyProp.AddNode(assgn2);
            taCodeCopyProp.AddNode(assgn3);
            taCodeCopyProp.AddNode(assgn4);
            taCodeCopyProp.AddNode(assgn5);
            taCodeCopyProp.AddNode(assgn6);

			var optCopyProp = new CopyPropagation();
			optCopyProp.Optimize(taCodeCopyProp.CodeList.ToList(), out var applCopProp);

			Assert.AreEqual(assgn2.Right, assgn1.Right);
			Assert.AreEqual(assgn4.Right, assgn1.Right);
			Assert.AreNotSame(assgn6.Right, assgn1.Right);
			Assert.True(true);
        }
```
