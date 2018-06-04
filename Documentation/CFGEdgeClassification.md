### Название задачи
Классификация рёбер CFG.
#### Постановка задачи
Дан ControlFlowGraph. Все его рёбра необходимо классифицировать на три группы:
1. Наступающие (coming) рёбра идут от узла к его истинному потомку.
2. Отступающие (retreating) рёбра идут от узла к его предку.
3. Поперечные (coming) - все остальные рёбра.

Пример классификации:
![Пример классификации рёбер CFG](http://graphonline.ru/tmp/saved/Dp/DpbOHbEOfFjCrfzY.png)
Рёбра 0 → 1 и 1 → 2 являются наступающими, 1 → 0 - отступающее, и 3 → 1 поперечное.
#### Зависимости задач в графе задач
Задачи, от которых зависит текущая задача:
1. Построение ControlFlowGraph.
2. Построение глубинного остовного дерева.

Задачи, зависящие от текущей:
1. Проверка CFG на приводимость

#### Теоретическая часть задачи
Для решения данной задачи строится глубинное остовное дерево - обход "поиск в глубину" вершин ControlFlowGraph, начиная с первой вершины. Отметим, что в процессе обхода графа вершину нумеруются согласно  Те рёбра, которые попали в данный граф, являются наступающими.

Ребро из вершины **x** в вершину **y** будет являться отступающим в том случае, если вершина **y** является предком вершины **x** (или номер вершины **x** больше номера вершины **y**).

#### Практическая часть задачи (реализация)
Вначале был создал класс-перечисление, в котором определяются классы рёбер:

```cs
public enum EdgeType
{
    Coming = 1,
    Retreating = 2,
    Cross = 3
}
```
Дополнительно был создан класс EdgeTypes, являющийся словарём, где ключ - ребро ControlFlowGraph, а значение - тип ребра.
```cs
public class EdgeTypes : Dictionary<Edge<BasicBlock>, EdgeType>
    {
        public override string ToString()
        {
            return string.Join("\n", this.Select(ed => $"[{ed.Key.Source.ToString()} -> {ed.Key.Target.ToString()}]: {ed.Value}"));
        }
    }
```

Наконец, в самом классе ControlFlowGraph добавлено поле EdgeTypes, где и хранится результат классификации. 
#### Тесты
Возьмём ControlFlowGraph из примера выше:
![Тест](http://graphonline.ru/tmp/saved/Dp/DpbOHbEOfFjCrfzY.png)

Результат работы программы:
0 → 1 : Coming
1 → 2: Coming
0 → 3: Coming
1 → 0: Retreating
3 → 1: Cross
#### Пример работы.
Приведём алгоритм работы данной задачи:
```cs
public void ClassificateEdges()
        {
            var depthTree = new DepthSpanningTree(this);
            foreach (var edge in CFGAuxiliary.Edges)
            {
                if (depthTree.SpanningTree.Edges.Any(e => e.Target.Equals(edge.Target) && e.Source.Equals(edge.Source)))
                {
                    EdgeTypes.Add(edge, EdgeType.Coming);
                }
                else if (depthTree.FindBackwardPath(edge.Source, edge.Target))
                {
                    EdgeTypes.Add(edge, EdgeType.Retreating);
                }
                else
                {
                    EdgeTypes.Add(edge, EdgeType.Cross);
                }
            }
        }
```
Метод для вызова в IDE:
```cs
public string PrintCFGEdgeClassification(ControlFlowGraph controlFlowGraph)
        {
            controlFlowGraph.ClassificateEdges();
            return controlFlowGraph.EdgeTypes.ToString();
        }
```
