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

    public class LabeledNode : StatementNode
    {
        public StatementNode Stat { get; set; }
        public IdNode Label { get; set; }

        public LabeledNode(IdNode label, StatementNode stat)
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
        public ExprNode Condition { get; set; }
        public StatementNode Body { get; set; }
        public CycleNode(ExprNode expr, StatementNode stat)
        {
            Condition = expr;
            Body = stat;
        }
    }

    public class BlockNode : StatementNode
    {
        public List<StatementNode> StList { get; set; }
        public BlockNode(StatementNode stat)
        {
            StList = new List<StatementNode>();
            StList.Add(stat);
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
        public List<ExprNode> ExprList { get; set; }
        public ExprListNode(ExprNode expr)
        {
            ExprList = new List<ExprNode>();
            ExprList.Add(expr);
        }
        public void Add(ExprNode expr)
        {
            ExprList.Add(expr);
        }
    }

    public class IfNode : StatementNode
    {
        public ExprNode Conditon { get; set; }
        public StatementNode IfClause { get; set; }
        public StatementNode ElseClause { get; set; }
        public IfNode(ExprNode expr, StatementNode ifClause, StatementNode elseClause = null)
        {
            Conditon = expr;
            IfClause = ifClause;
            ElseClause = elseClause;
        }
    }

    public class ForNode : StatementNode
    {
        public AssignNode Assign { get; set; }
        public ExprNode Border { get; set; }
        public ExprNode Inc { get; set; }
        public StatementNode Body { get; set; }
        public ForNode(AssignNode assign, ExprNode bord, ExprNode inc, StatementNode body)
        {
            Assign = assign;
            Border = bord;
            Inc = inc;
            Body = body;
        }

        public ForNode(AssignNode assign, ExprNode bord, StatementNode body): this(assign, bord, new IntNumNode(1), body) {}
    }

    public class EmptyNode : StatementNode
    {
    }


}