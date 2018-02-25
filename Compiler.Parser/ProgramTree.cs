using System.Collections.Generic;

namespace Compiler.Parser.AST
{
    public enum OperationType
    {
        Plus, Minus, Mul, Div,
        Greater, Less, GreaterEq, LessEq, Equal, NotEqual,
        Not, 
        UnaryMinus
    }

    public class Node // базовый класс для всех узлов    
    {
    }

    public class ExprNode : Node // базовый класс для всех выражений
    {
    }

    public class IdNode : ExprNode
    {
        public string Name { get; set; }
        public IdNode(string name) { Name = name; }
    }

    public class IntNumNode : ExprNode
    {
        public int Num { get; set; }
        public IntNumNode(int num) { Num = num; }
    }

    public class BinaryNode : ExprNode
    {
        public ExprNode Left { get; set; }
        public ExprNode Right { get; set; }
        public OperationType Operation { get; set; }
        public BinaryNode(ExprNode left, ExprNode right, OperationType op)
        {
            Left = left;
            Right = right;
            Operation = op;
        }
    }

    public class UnaryNode : ExprNode
    {
        public ExprNode Num { get; set; }
        public OperationType Operation { get; set; }
        public UnaryNode(ExprNode num, OperationType op)
        {
            Num = num;
            Operation = op;
        }
        public UnaryNode(int num, OperationType op) : this(new IntNumNode(num), op) {}
    }

    public class StatementNode : Node // базовый класс для всех операторов
    {
    }

    public class LabelNode : StatementNode
    {
        public StatementNode Stat { get; set; }
        public IdNode Label { get; set; }

        public LabelNode(IdNode label, StatementNode stat)
        {
            Label = label;
            Stat = stat;
        }
    }

    public class GoToNode : StatementNode
    {
        public IdNode Label { get; set; }

        public GoToNode(IdNode label)
        {
            Label = label;
        }
    }

    public class AssignNode : StatementNode
    {
        public IdNode Id { get; set; }
        public ExprNode Expr { get; set; }
        public AssignNode(IdNode id, ExprNode expr)
        {
            Id = id;
            Expr = expr;
        }
    }

    public class CycleNode : StatementNode
    {
        public ExprNode Expr { get; set; }
        public StatementNode Stat { get; set; }
        public CycleNode(ExprNode expr, StatementNode stat)
        {
            Expr = expr;
            Stat = stat;
        }
    }

    public class BlockNode : StatementNode
    {
        public List<StatementNode> StList = new List<StatementNode>();
        public BlockNode(StatementNode stat)
        {
            Add(stat);
        }
        public void Add(StatementNode stat)
        {
            StList.Add(stat);
        }
    }

    public class PrintNode : StatementNode
    {
        public ExprListNode ExprList { get; set; }
        public PrintNode(ExprListNode exprlist)
        {
            ExprList = exprlist;
        }
    }

    public class ExprListNode : Node
    {
        public List<ExprNode> ExpList = new List<ExprNode>();
        public ExprListNode(ExprNode exp)
        {
            Add(exp);
        }
        public void Add(ExprNode exp)
        {
            ExpList.Add(exp);
        }
    }

    public class IfNode : StatementNode
    {
        public ExprNode Expr { get; set; }
        public StatementNode Stat1 { get; set; }
        public StatementNode Stat2 { get; set; }
        public IfNode(ExprNode expr, StatementNode stat1, StatementNode stat2)
        {
            Expr = expr;
            Stat1 = stat1;
            Stat2 = stat2;
        }

        public IfNode(ExprNode expr, StatementNode stat1)
        {
            Expr = expr;
            Stat1 = stat1;
            Stat2 = null;
        }
    }

    public class ForNode : StatementNode
    {
        public AssignNode Assign { get; set; }
        public ExprNode Cond { get; set; }
        public ExprNode Inc { get; set; }
        public StatementNode Stat { get; set; }
        public ForNode(AssignNode assign, ExprNode cond, ExprNode inc, StatementNode stat)
        {
            Assign = assign;
            Cond = cond;
            Inc = inc;
            Stat = stat;
        }

        public ForNode(AssignNode assign, ExprNode cond, StatementNode stat): this(assign, cond, null, stat) {}
    }

    public class EmptyNode : StatementNode
    {
    }


}