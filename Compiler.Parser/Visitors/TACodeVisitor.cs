using System;
using Compiler.Parser.AST;
using Compiler.ThreeAddressCode;

namespace Compiler.Parser.Visitors
{
    public class TACodeVisitor : AutoVisitor
    {
        private TACode code = new TACode();

        public override void VisitBinaryNode(BinaryNode binop)
        {
            var assign = code.AddAssign();
            
            if (binop.Left.GetType() == typeof(IdNode))
            {
                var tmp = (IdNode) binop.Left;
                assign.Left = code.GetVarByName(tmp.Name);
            }
            else if (binop.Left.GetType() == typeof(IntNumNode))
            {
                var tmp = (IntNumNode) binop.Left;
                assign.Left = code.GetConst(tmp.Num);
            }
            else if (binop.Left.GetType() == typeof(BinaryNode) || binop.Left.GetType() == typeof(UnaryNode) )
            {
                assign.Left = code.GetTempVar();
                binop.Left.Visit(this);
            }
            
            if (binop.Right.GetType() == typeof(IdNode))
            {
                var tmp = (IdNode) binop.Left;
                assign.Right = code.GetVarByName(tmp.Name);
            }
            else if (binop.Right.GetType() == typeof(IntNumNode))
            {
                var tmp = (IntNumNode) binop.Left;
                assign.Right = code.GetConst(tmp.Num);
            } 
            else if (binop.Right.GetType() == typeof(BinaryNode) || binop.Right.GetType() == typeof(UnaryNode) )
            {
                assign.Right = code.GetTempVar();
                binop.Right.Visit(this);
            }

            assign.Result = code.GetTempVar();
            if (Enum.TryParse(binop.Operation, out OpCode op))
            {
                assign.Operation = op;
            }
        }

        public override void VisitUnaryNode(UnaryNode unop)
        {
            var assign = code.AddAssign();
            assign.Left = null;
            
            if (unop.Num.GetType() == typeof(IdNode))
            {
                var tmp = (IdNode) unop.Num;
                assign.Right = code.GetVarByName(tmp.Name);
            }
            else if (unop.Num.GetType() == typeof(IntNumNode))
            {
                var tmp = (IntNumNode) unop.Num;
                assign.Right = code.GetConst(tmp.Num);
            }
            else if (unop.Num.GetType() == typeof(BinaryNode) || unop.Num.GetType() == typeof(UnaryNode))
            {
                assign.Right = code.GetTempVar();
                unop.Num.Visit(this);
            }
            
            assign.Result = code.GetTempVar();
            if (Enum.TryParse(unop.Operation.ToString(), out OpCode op)) // Может сделать операцию string как и везде?
            {
                assign.Operation = op;
            }
        }

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

            if (a.Expr.GetType() == typeof(IdNode))
            {
                var tmp = (IdNode) a.Expr;
                assign.Right = code.GetVarByName(tmp.Name);
            }
            else if (a.Expr.GetType() == typeof(IntNumNode))
            {
                var tmp = (IntNumNode) a.Expr;
                assign.Right = code.GetConst(tmp.Num);
            }
            else if (a.Expr.GetType() == typeof(BinaryNode))
            {
                var tmp = (BinaryNode) a.Expr;
                VisitBinaryNode(tmp);
            }
            else
            {
                assign.Right = code.GetTempVar();
                var tmp = (UnaryNode) a.Expr;
                VisitUnaryNode(tmp);
            }
            
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
    }
}