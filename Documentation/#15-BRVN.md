### Название задачи
Применение алгебраических и логических тождеств.
#### Постановка задачи
Реализовать оптимизацию применение алгебраических и логических тождеств для базового блока.
#### Зависимости задач в графе задач
Зависит от:
* Трехадресный код

#### Теоретическая часть задачи
В коде программы могут быть применены следующие тождества для оптимизации:  
```csharp
x + 0 = x        x * 1 = x
0 + x = x        1 * x = x
x - 0 = x        x * 0 = 0
x - x = 0        x / 1 = x
                 x / x = 1
```

#### Практическая часть задачи (реализация)
Часть кода для оптимизации операции сложения. Полный файл по [ссылке](https://github.com/Lucky112/mmcs-optimizing-compiler-spring-2018/blob/master/Compiler.Optimizations/AlgebraicOptimization.cs).
```csharp
public List<Node> Optimize(List<Node> nodes, out bool applied)
{
    var app = false;
    var enumerable = nodes
        .OfType<Assign>()
        .Where(assn => assn.Operation != OpCode.Copy && assn.Left != null);
    foreach (var node in enumerable)
    {
        switch (node.Operation)
        {
            case OpCode.Plus:
                if (node.Left.Equals(Zero))
                    app = SetLeft(node);
                else if (node.Right.Equals(Zero))
                    app = SetRight(node);
                break;
            ...
        }
    }
    ...
}
```

#### Тесты
(в трехадресном коде)
```csharp
l0: a = b + 0 => l0: a = b
l1: b = 1 * a => l1: b = a
```

#### Пример работы.
(в трехадресном коде)
```csharp
l0: a = b + 0 
l1: a = a * 1 
l2: a = a - 0 
l3: a = a + b 
l4: a = a * 1 

=>

l0: a = b
l1: a = a
l2: a = a
l3: a = a + b
l4: a = a
```



