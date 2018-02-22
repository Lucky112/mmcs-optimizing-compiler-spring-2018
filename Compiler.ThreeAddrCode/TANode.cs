using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        public override string ToString()
        {
            return string.Format("{0} : nop", Label);
        }

        public override string ToString()
        {
            if (Left == null)
                return string.Format("{0} : {0} = {1}{2}", Label, Result, Operation.GetSymbol(), Right);
            else
                return string.Format("{0} : {0} = {1} {2} {3}", Label, Result, Left, Operation.GetSymbol(), Right);
        }

        public override string ToString()
        {
            return string.Format("{0} : goto {1}", Label, TargetLabel);
        }
    /// <summary>
    /// Базовый класс для трехадресного кода
    /// </summary>
    public abstract class TANode
    {
        public abstract TAExpr Left { get; set; }
        public abstract TAExpr Right { get; set; }
        public abstract TAExpr Result { get; set; }
        public abstract string Operation { get; set; }
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
    public class TAExpr
    {
    }

    /// <summary>
    /// Операнд-константа (числа типа int)
    /// </summary>
    public class IntConst : TAExpr
    {
        public int Num { get; set; }
        public IntConst(int num) { Num = num; }

        public override string ToString()
        {
            return Num.ToString();
        }
    }

    /// <summary>
    /// Операнд-временного имени
    /// </summary>
    public class GenName : TAExpr
    {
        public string Name { get; set; }
        public GenName(string name) { Name = name; }

        public override string ToString()
        {
            return ID.ToString();
        }
    }

    /*
     * =========================================================
     * Код программы хранится в виде команд 
     * в формате трехадресного кода в массиве
     * =========================================================
    */

    /// <summary>
    /// Класс для хранения программы в формате 
    /// трехадресного кода
    /// </summary>
    public class TACode
    {
        List<TANode> table;

        public override string ToString()
        {
            return m_code.Aggregate("", (s, node) => s + node.ToString() + Environment.NewLine);
        }
    }
}
