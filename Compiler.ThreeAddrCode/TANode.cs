using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.Parser.Visitors;
using Compiler.Parser.AST;

namespace Compiler.ThreeAddressCode
{
    /*
     * =========================================================
     * Трехадресный код имеет следующий вид:
     * Result := Left Operation Right
     * Result, Left, Right - операнды
     * Operation - бинарная операция 
     * =========================================================
    */

    public enum OpCode
    {
        TA_Plus,
        TA_Minus,
        TA_Mul,
        TA_Div,

        TA_UnaryMinus,
        TA_Copy
    }

    public static class OpCodeExt
    {
        public static string GetSymbol(this OpCode op)
        {
            switch (op)
            { 
                case OpCode.TA_Plus: return "+";
                case OpCode.TA_UnaryMinus: 
                case OpCode.TA_Minus: return "-";
                case OpCode.TA_Mul: return "*";
                case OpCode.TA_Div: return "/";
                case OpCode.TA_Copy: return "";

                default: return "unknown";
            }
        }
    }

    /// <summary>
    /// Базовый узел
    /// </summary>
    public class TA_Node
    {        
        /// <summary>
        /// Уникальная метка-идентификатор
        /// </summary>
        public Guid Label { get; } 

        public TA_Node() { Label = Guid.NewGuid(); }        
    }

    /// <summary>
    /// Пустой оператор
    /// </summary>
    public class TA_Empty : TA_Node
    {
        public override string ToString()
        {
            return string.Format("{0} : nop", Label);
        }
    }

    /// <summary>
    /// Оператор присваивания
    /// </summary>
    public class TA_Assign : TA_Node
    {
        /// <summary>
        /// Левый операнд (null, если операция унарная)
        /// </summary>
        public TA_Expr Left { get; set; }

        /// <summary>
        /// Правый операнд
        /// </summary>
        public TA_Expr Right { get; set; }

        /// <summary>
        /// Хранилище результата
        /// </summary>
        public TA_Var Result { get; set; }

        /// <summary>
        /// Производимая операция
        /// </summary>
        public OpCode Operation { get; set; }

        public override string ToString()
        {
            if (Left == null)
                return string.Format("{0} : {0} = {1}{2}", Label, Result, Operation.GetSymbol(), Right);
            else
                return string.Format("{0} : {0} = {1} {2} {3}", Label, Result, Left, Operation.GetSymbol(), Right);
        }
    }

    /// <summary>
    /// Оператор безусловного перехода
    /// </summary>
    public class TA_Goto : TA_Node
    {
        /// <summary>
        /// Метка-идентификатор оператора, на который происходит переход
        /// </summary>
        public Guid TargetLabel { get; set; }

        public override string ToString()
        {
            return string.Format("{0} : goto {1}", Label, TargetLabel);
        }
    }


    /// <summary>
    /// Оператор условного перехода
    /// </summary>
    public class TA_IfGoto : TA_Goto
    {
        /// <summary>
        /// Условие перехода
        /// </summary>
        public TA_Expr Condition { get; set; }
        public override string ToString()
        {
            return string.Format("{0} : if {1} goto {2}", Label, Condition, TargetLabel);
        }
    }


    /*
     * =========================================================
     * Операнды могут быть либо константами,
     * либо сгенерированными переменными
     * =========================================================
    */

    /// <summary>
    /// Базовый класс для операндов
    /// </summary>
    public class TA_Expr
    {
    }

    /// <summary>
    /// Операнд-константа (числа типа int)
    /// </summary>
    public class IntConst : TA_Expr
    {
        public int Num { get; }

        public IntConst(int num) { Num = num; }

        public override string ToString()
        {
            return Num.ToString();
        }
    }

    /// <summary>
    /// Операнд-переменная
    /// </summary>
    public class TA_Var : TA_Expr
    {
        public Guid ID { get; }
        public string Name { get; set; }

        public TA_Var() { ID = Guid.NewGuid(); }

        public override string ToString()
        {
            return ID.ToString();
        }
    }

    

    /*
     * =========================================================
     * Код программы хранится в виде команд 
     * в формате трехадресного кода в списке
     * =========================================================
    */

    /// <summary>
    /// Класс для хранения программы в формате 
    /// трехадресного кода
    /// </summary>
    public class TACode
    {
        List<TA_Node> m_code;

        Dictionary<Guid, TA_Node> m_labeledCode;
        Dictionary<string, TA_Var> m_varsInCode;
        
        public TACode()
        {
            m_code = new List<TA_Node>();
            m_labeledCode = new Dictionary<Guid, TA_Node>();
            m_varsInCode = new Dictionary<string, TA_Var>();
        }

        /// <summary>
        /// Добавить пустой оператор
        /// </summary>
        public TA_Empty AddNop()
        {
            var curNode = new TA_Empty();
            m_code.Add(curNode);
            m_labeledCode.Add(curNode.Label, curNode);

            return curNode;
        }

        /// <summary>
        /// Добавить оператор присваивания
        /// </summary>
        public TA_Assign AddAssign()
        {
            var curNode = new TA_Assign();
            m_code.Add(curNode);
            m_labeledCode.Add(curNode.Label, curNode);

            return curNode;
        }

        /// <summary>
        /// Добавить оператор безусловного перехода
        /// </summary>
        public TA_Goto AddGoto()
        {
            var curNode = new TA_Goto();
            m_code.Add(curNode);
            m_labeledCode.Add(curNode.Label, curNode);

            return curNode;
        }
        
        /// <summary>
        /// Добавить оператор условного перехода
        /// </summary>
        public TA_IfGoto AddIfGoto()
        {
            var curNode = new TA_IfGoto();
            m_code.Add(curNode);
            m_labeledCode.Add(curNode.Label, curNode);

            return curNode;
        }

        /// <summary>
        /// Найти переменную по имени в исходном коде
        /// </summary>
        public TA_Var GetVarByName(string name)
        {
            if (!m_varsInCode.ContainsKey(name))
                m_varsInCode.Add(name, new TA_Var());

            return m_varsInCode[name];
        }

        /// <summary>
        /// Создать временную переменную
        /// </summary>
        public TA_Var GetTempVar()
        {
            return new TA_Var();
        }


        public override string ToString()
        {
            return m_code.Aggregate("", (s, node) => s + node.ToString() + Environment.NewLine);
        }
    }
}
