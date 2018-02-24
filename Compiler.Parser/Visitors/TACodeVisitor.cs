using System;
using Compiler.Parser.AST;
using Compiler.ThreeAddressCode;

namespace Compiler.Parser.Visitors
{
    public class TACodeVisitor : AutoVisitor
    {
        private TACode code = new TACode();

        // TODO
        public override void VisitLabelNode(LabelNode l)
        {
            base.VisitLabelNode(l);
        }

        public override void VisitGoToNode(GoToNode g)
        {
            var gt = code.AddGoto();
            var target = code.GetVarByName(g.Label.Name);
            gt.TargetLabel = target.ID;
        }

        public override void VisitAssignNode(AssignNode a)
        {   
            var assign = code.AddAssign();
            assign.Left = null;
            assign.Right = RecAssign(a.Expr);
            assign.Result = code.GetVarByName(a.Id.Name);
        }

        // TODO
        public override void VisitCycleNode(CycleNode c)
        {
            base.VisitCycleNode(c);
        }

        // TODO
        public override void VisitBlockNode(BlockNode bl)
        {
            base.VisitBlockNode(bl);
        }

        // TODO
        public override void VisitPrintNode(PrintNode pr)
        {
            base.VisitPrintNode(pr);
        }

        // TODO
        public override void VisitExprListNode(ExprListNode exn)
        {
            base.VisitExprListNode(exn);
        }

        // TODO
        public override void VisitIfNode(IfNode iif)
        {
            base.VisitIfNode(iif);
        }

        // TODO
        public override void VisitForNode(ForNode f)
        {
            base.VisitForNode(f);
        }

        public override void VisitEmptyNode(EmptyNode w)
        {
            code.AddNop();
        }
        
        private TA_Var RecAssign(ExprNode ex)
        {
            var assign = code.AddAssign();
            var result = new TA_Var();
            assign.Result = result;

            switch (ex)
            {
                case IdNode tmp1:
                    assign.Left = null;
                    assign.Right = code.GetVarByName(tmp1.Name);
                    return result;
                case IntNumNode tmp2:
                    assign.Left = null;
                    assign.Right = code.GetConst(tmp2.Num);
                    return result;
                case BinaryNode tmp3:
                    assign.Left = RecAssign(tmp3.Left);
                    assign.Right = RecAssign(tmp3.Right);
                    if (Enum.TryParse(tmp3.Operation, out OpCode op1))
                    {
                        assign.Operation = op1;
                    }       
                    return result;
                case UnaryNode tmp4:
                    assign.Left = null;
                    assign.Right = RecAssign(tmp4.Num);
                    if (Enum.TryParse(tmp4.Operation.ToString(), out OpCode op2))
                    {
                        assign.Operation = op2;
                    }
                    return result;
            }

            return result;
        }
    }
}