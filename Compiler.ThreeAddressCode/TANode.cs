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

    /// <summary>
    /// Базовый класс для трехадресного кода
    /// </summary>
    public abstract class TANode
    {
        public abstract TAExpr Left { get; set; }
        public abstract TAExpr Right { get; set; }
        public abstract TAExpr Result { get; set; }
        public abstract string Operation { get; set; }
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
    }

    /// <summary>
    /// Операнд-временного имени
    /// </summary>
    public class GenName : TAExpr
    {
        public string Name { get; set; }
        public GenName(string name) { Name = name; }
    }

    /*
     * =========================================================
     * Код программы храниться в виде команд 
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
    }
}
