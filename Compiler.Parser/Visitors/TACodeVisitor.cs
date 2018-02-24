using System;
using Compiler.Parser.AST;
using Compiler.ThreeAddressCode;
using System.Collections.Generic;

namespace Compiler.Parser.Visitors
{
    public class TACodeVisitor : AutoVisitor
    {
        private TACode code = new TACode();

        /// <summary>
        /// Список помеченных команд(строк) трехадресного кода в формате ИмяМетки - Узел
        /// </summary>
        private Dictionary<string, TA_Node> labeledTANodes = new Dictionary<string, TA_Node>();
        /// <summary>
        /// Список команд безусловного перехода, ведущих в непреобразованную часть кода и ожидающих заполнения поля TargetLabel, в формате ИмяМетки - Список команд перехода к этой метке
        /// </summary>
        private Dictionary<string, List<TA_Goto>> forwardGotos = new Dictionary<string, List<TA_Goto>>();
        

        public override void VisitLabelNode(LabelNode l)
        {
            string labelName = l.Label.Name;
            // Создаем пустой оператор и указываем, что на него есть переход по метке
            TA_Empty labeledNop = code.AddNop();
            labeledNop.IsLabeled = true;

            // Добавляем метку и помеченный оператор в список помеченных операторов (это всегда нужно делать, т.к. дальше по тексту могут оказаться goto на данную метку)
            labeledTANodes.Add(labelName, labeledNop);

            // Проверяем, не было ли по этой метке переходов, преобразованных ранее
            if (forwardGotos.ContainsKey(labelName))
            {
                // Если были, заполняем их поля меток и удаляем из списка ожидания
                foreach (var ta_goto in forwardGotos[labelName])
                    ta_goto.TargetLabel = labeledNop.Label;
                forwardGotos.Remove(labelName);
            }

            // Продолжаем отдельно разбор помеченного оператора как обычного
            l.Stat.Visit(this);
        }

        public override void VisitGoToNode(GoToNode g)
        {
            string labelName = g.Label.Name;
            // При посещении узла GoTо создаем соответсвующую команду трехадресного кода
            TA_Goto gt = code.AddGoto();

            // Если метка ведет в уже преобразованную часть программы
            if (labeledTANodes.ContainsKey(labelName))
            {
                // Получаем помеченную строку треадресного кода и задаем ее как цель перехода
                TA_Node target = labeledTANodes[labelName];
                gt.TargetLabel = target.Label;
            }
            else
            {
                // Иначе помещаем строку в лист ожидания пока помеченная часть программы не будет преобразована
                if (!forwardGotos.ContainsKey(labelName))
                    forwardGotos.Add(labelName, new List<TA_Goto>());
                forwardGotos[labelName].Add(gt);
            }
        }

        public override void VisitAssignNode(AssignNode a)
        {   
            var assign = code.AddAssign();
            assign.Left = null;
            assign.Right = RecAssign(a.Expr);
            assign.Result = code.GetVarByName(a.Id.Name);
            assign.Operation = OpCode.TA_Copy;
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
                    assign.Operation = OpCode.TA_Copy;
                    return result;
                case IntNumNode tmp2:
                    assign.Left = null;
                    assign.Right = code.GetConst(tmp2.Num);
                    assign.Operation = OpCode.TA_Copy;
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