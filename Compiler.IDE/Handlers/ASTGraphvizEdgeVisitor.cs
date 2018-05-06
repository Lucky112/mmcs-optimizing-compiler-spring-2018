using Compiler.Parser.AST;
using Compiler.Parser.Visitors;
using System.Text;

namespace Compiler.IDE.Handlers
{
    class ASTGraphvizEdgeVisitor : IVisitor
    {
        public string Text => _text.ToString();
        private StringBuilder _text = new StringBuilder();

        public void VisitAssignNode(AssignNode a)
        {
            throw new System.NotImplementedException();
        }

        public void VisitBinaryNode(BinaryNode binop)
        {
            throw new System.NotImplementedException();
        }

        public void VisitBlockNode(BlockNode bl)
        {
            throw new System.NotImplementedException();
        }

        public void VisitCycleNode(CycleNode c)
        {
            throw new System.NotImplementedException();
        }

        public void VisitEmptyNode(EmptyNode w)
        {
            throw new System.NotImplementedException();
        }

        public void VisitExprListNode(ExprListNode el)
        {
            throw new System.NotImplementedException();
        }

        public void VisitExprNode(ExprNode node)
        {
            throw new System.NotImplementedException();
        }

        public void VisitForNode(ForNode w)
        {
            throw new System.NotImplementedException();
        }

        public void VisitGoToNode(GoToNode g)
        {
            throw new System.NotImplementedException();
        }

        public void VisitIdNode(IdNode id)
        {
            throw new System.NotImplementedException();
        }

        public void VisitIfNode(IfNode iif)
        {
            throw new System.NotImplementedException();
        }

        public void VisitIntNumNode(IntNumNode num)
        {
            throw new System.NotImplementedException();
        }

        public void VisitLabeledNode(LabeledNode l)
        {
            throw new System.NotImplementedException();
        }

        public void VisitPrintNode(PrintNode p)
        {
            throw new System.NotImplementedException();
        }

        public void VisitUnaryNode(UnaryNode unop)
        {
            throw new System.NotImplementedException();
        }
    }
}
