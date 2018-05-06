using Compiler.Parser.AST;
using Compiler.Parser.Visitors;
using System.Runtime.Serialization;
using System.Text;

namespace Compiler.IDE.Handlers
{
    class ASTGraphvizNodeVisitor : IVisitor
    {
        private ObjectIDGenerator gen = new ObjectIDGenerator();

        public string Text => _text.ToString();
        private StringBuilder _text = new StringBuilder();

        private string GetLabel(object node)
        {
            return gen.GetId(node, out bool b).ToString();
        }

        private string GetText(object a)
        {
            return $"{GetLabel(a)}  [label = \"{a.ToString()}\"]\n";
        }

        public void VisitAssignNode(AssignNode a)
        {   
            _text.Append($"{GetLabel(a)}  [label = \"{a}\"]\n");
            a.Id.Visit(this);
            a.Expr.Visit(this);
        }

        public void VisitBinaryNode(BinaryNode a)
        {
            _text.Append($"{GetLabel(a)}  [label = \"{a}\"]\n");
            a.Left.Visit(this);
            a.Right.Visit(this);
        }

        public void VisitBlockNode(BlockNode a)
        {
            _text.Append($"{GetLabel(a)}  [label = \"{a}\"]\n");
            foreach (var e in a.StList)
                e.Visit(this);
        }

        public void VisitCycleNode(CycleNode a)
        {
            _text.Append($"{GetLabel(a)}  [label = \"{a}\"]\n");
            a.Condition.Visit(this);
            a.Body.Visit(this);
        }

        public void VisitEmptyNode(EmptyNode a)
        {
            _text.Append($"{GetLabel(a)}  [label = \"{a}\"]\n");
        }

        public void VisitExprListNode(ExprListNode a)
        {
            _text.Append($"{GetLabel(a)}  [label = \"{a}\"]\n");
            foreach (var e in a.ExprList)
                e.Visit(this);
        }

        public void VisitExprNode(ExprNode a)
        {
            _text.Append($"{GetLabel(a)}  [label = \"{a}\"]\n");
            //a.Visit(this);
        }

        public void VisitForNode(ForNode a)
        {
            _text.Append($"{GetLabel(a)}  [label = \"{a}\"]\n");
            a.Assign.Visit(this);
            a.Border.Visit(this);
            a.Inc.Visit(this);
            a.Body.Visit(this);
        }

        public void VisitGoToNode(GoToNode a)
        {
            _text.Append($"{GetLabel(a)}  [label = \"{a}\"]\n");
        }

        public void VisitIdNode(IdNode a)
        {
            _text.Append($"{GetLabel(a)}  [label = \"{a}\"]\n");
        }

        public void VisitIfNode(IfNode a)
        {
            _text.Append($"{GetLabel(a)}  [label = \"{a}\"]\n");
            a.IfClause.Visit(this);
            a.Conditon.Visit(this);
            a.ElseClause?.Visit(this);
        }

        public void VisitIntNumNode(IntNumNode a)
        {
            _text.Append($"{GetLabel(a)}  [label = \"{a}\"]\n");
        }

        public void VisitLabeledNode(LabeledNode a)
        {
            _text.Append($"{GetLabel(a)}  [label = \"{a}\"]\n");
            a.Stat.Visit(this);
        }

        public void VisitPrintNode(PrintNode a)
        {
            _text.Append($"{GetLabel(a)}  [label = \"{a}\"]\n");
        }

        public void VisitUnaryNode(UnaryNode a)
        {
            _text.Append($"{GetLabel(a)}  [label = \"{a}\"]\n");
        }
    }
}
