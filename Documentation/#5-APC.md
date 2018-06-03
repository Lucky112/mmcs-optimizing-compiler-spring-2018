### Название задачи
Реализация Pretty Print визитора
#### Постановка задачи
Необходимо реализовать Pretty Print визитор, который по AST 
восстанавливает отформатированный исходный код программы.
#### Зависимости задач в графе задач
Зависит от:
* AST-дерево

#### Теоретическая часть задачи
Для восстановления исходного кода программы по AST будем совершать обход дерева,
накопливая код в поле визитора Text, при этом учитывая тип посещаемго узла. 
Для этого класс PrettyPrintVisitor реализует интерфейс Ivisitor.
Отступы поддерживаются с помощью переменной Indent, которая увеличивается на 2 
при входе в блок и уменьшается на 2 перед выходом из блока. 

#### Практическая часть задачи (реализация)
Набор методов класса ```PrettyPrintVisitor```, реализующих ```Ivisitor```:

```csharp
0.  VisitIdNode(IdNode id)
1.  VisitIntNumNode(IntNumNode num)
2.  VisitUnaryNode(UnaryNode unop)
3.  VisitBinaryNode(BinaryNode binop)
4.  VisitAssignNode(AssignNode a)
5.  VisitCycleNode(CycleNode c)
6.  VisitBlockNode(BlockNode bl)
7.  VisitPrintNode(PrintNode p)
8.  VisitGoToNode(GoToNode g)
9.  VisitLabeledNode(LabeledNode l)
10. VisitExprListNode(ExprListNode el)
11. VisitIfNode(IfNode iif)
12. VisitForNode(ForNode w)
13. VisitEmptyNode(EmptyNode w)
14. VisitExprNode(ExprNode s)
```
Пример реализации ```VisitExprListNode```:
```csharp
public override void VisitExprListNode(ExprListNode el) {
     var last = el.ExprList.Last();
     el.ExprList.ForEach(expr => 
         {
            expr.Visit(this);
               if (expr != last)
                   Text += ",";
         });
 }
```
#### Пример работы.
Исходный код:
```csharp
if (1 < -3)
  a = 123;
else
  goto h;
for(i = 0, 10)
{
 print(1 >= 3);
 if (1 + 3)
 {
   a = 1;
 }
}
h: {c = a + b;}
```

Исходный код, восстановленный через PrettyPrintVisitor:
```csharp
if ((1 < - 3))
  a = 123;
else
  goto h;
for(i = 0,10,1)
{
  print((1 >= 3));
  if ((1 + 3))
  {
    a = 1;
  }
}
h:
{
  c = (a + b);
}
```


