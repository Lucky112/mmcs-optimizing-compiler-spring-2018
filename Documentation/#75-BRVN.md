### Название задачи
Обобщённый итерационный алгоритм

#### Постановка задачи
Реализовать обобщённый итерационный алгоритм

#### Зависимости задач в графе задач
Задача зависит от:
* Control Flow Graph
* Интерфейс передаточной функции

#### Теоретическая часть задачи
<img src="https://preview.ibb.co/gOa4YJ/04_D95_E034_BAE0_CEB735_F6_FD72_D43_E51443_E1_CD5_FBDEB916_B3_B_pimgpsh_fullsize_distr.jpg" width="500" height="340" />

#### Практическая часть задачи (реализация)
```csharp
    public class GenericIterativeAlgorithm<T> : IAlgorithm<T>
    {
        public Func<T, T, bool> Comparer { get; set; }
        public Func<(T, T)> Fill { get; set; } // начальное значение (значение первого узла)
        public Func<(T,T), (T,T), bool> Finish { get; set; } // условие завершения итерационного алгоритма

        public InOutData<T> Analyze(
            ControlFlowGraph graph,
            ILatticeOperations<T> ops,
            ITransferFunction<T> f)
        {
            var data = new InOutData<T>();
            data[graph.CFGNodes.ElementAt(0)] = Fill();

            foreach (var node in graph.CFGNodes)
                data[node] = Fill();

            var outChanged = true;
            while (outChanged)
            {
                outChanged = false;
                foreach (var block in graph.CFGNodes)
                {
                    var inset = block.Parents.Aggregate(ops.Lower, (x, y)
                        => ops.Operator(x, data[y].Item2));
                    var outset = f.Transfer(block, inset, ops);
                    if (!Finish((inset,outset), data[block]))
                    {
                        outChanged = true;
                    }
                    data[block] = (inset, outset);
                }
            }
            return data;
        }
    }
```

#### Тесты
Для тестирования использовался случай достигающих опеределений.

```csharp
l1: a = 3 - 5
l2: b = 10 + 2
l3: c = -1
l_: if 1 goto l3
ass4 = l5: d = c + 1999
l_: if 2 goto l2
l7: e = 7 * 4
l8: f = 100 / 25
```

```csharp
IN[0] = { }
OUT[0] = { l1 }

IN[1] = { l1, l2, l3, l5 }
OUT[1] = { l1, l2, l3, l4 }

IN[2] = { l1, l2, l3, l5 },
OUT[2] = { l1, l2, l4, l5 }

IN[3] =  { l1, l2, l3, l5 }
OUT[3] =  { l1, l2, l3, l5 }

IN[4] =  { l1, l2, l3, l5 }
OUT[4] = {  l1, l2, l3, l5, l7, l8 }
```

#### Пример работы.
![](https://image.ibb.co/hjczy8/Capture.png)  
<img src="https://image.ibb.co/hAb6OJ/IMG_30052018_213638_0.jpg" width="540" height="140" />

