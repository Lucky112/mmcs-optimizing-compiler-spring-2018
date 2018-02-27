using Compiler.Parser.AST;

namespace Compiler.Parser.Visitors
{
    public interface IVisitor
    {
        void VisitIdNode(IdNode id);

        void VisitIntNumNode(IntNumNode num);

        void VisitBinaryNode(BinaryNode binop);

        void VisitUnaryNode(UnaryNode unop);

        void VisitLabeledNode(LabeledNode l);

        void VisitGoToNode(GoToNode g);

        void VisitAssignNode(AssignNode a);

        void VisitCycleNode(CycleNode c);

        void VisitBlockNode(BlockNode bl);

        void VisitPrintNode(PrintNode p);

        void VisitExprListNode(ExprListNode el);

        void VisitIfNode(IfNode iif);

        void VisitForNode(ForNode w);

        void VisitEmptyNode(EmptyNode w);

        void VisitLabelNode(LabeledNode l);
    }
}