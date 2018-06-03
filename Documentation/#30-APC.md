### Название задачи
Реализация передаточной функции композицией
#### Постановка задачи
f_s(X) - для кода  
f_b(X) - для блока  
X - множество из DFA  
OUT[B] = f_b(IN[B])  
Нужно уметь находить суперпозицию f_b = f_s1 . f_s2 . ... . f_sn
#### Зависимости задач в графе задач
Зависит от:
* Базовый блок

#### Теоретическая часть задачи
Отсутствует

#### Практическая часть задачи (реализация)
Набор методов класса ```PrettyPrintVisitor```, реализующих ```Ivisitor```:

```csharp
public static class TransferFunctionUtils
{
    public static ITransferFunction<T> Compose<T>(this ITransferFunction<T> f1, ITransferFunction<T> f2) {}
    private class TransferFuctionComposition<T> : ITransferFunction<T>
    {
        private readonly ITransferFunction<T> f1, f2;
        public TransferFuctionComposition(ITransferFunction<T> f1, ITransferFunction<T> f2) {}
        public T Transfer(BasicBlock basicBlock, T input, ILatticeOperations<T> ops) {}
    }
}
```
#### Пример работы.
Отсутствует