using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Compiler.Parser;

using Compiler.ThreeAddrCode.Nodes;
using Compiler.ThreeAddrCode.Expressions;
using Compiler.ThreeAddrCode.CFG;
using Compiler.ThreeAddrCode;

namespace Compiler
{
    class Program
    {
        public static void Main()
        {
            TACode ta_code = new TACode();

            Assign ass1 = new Assign();
            ass1.Left = new IntConst(3);
            ass1.Operation = OpCode.Minus;
            ass1.Right = new IntConst(5);
            ass1.Result = new Var();

            Assign ass2 = new Assign();
            ass2.Left = new IntConst(10);
            ass2.Operation = OpCode.Plus;
            ass2.Right = new IntConst(2);
            ass2.Result = new Var();

            Assign ass3 = new Assign();
            ass3.Operation = OpCode.Minus;
            ass3.Right = new IntConst(1);
            ass3.Result = new Var();

            IfGoto ifgt1 = new IfGoto();
            ifgt1.Condition = new IntConst(1);
            ifgt1.TargetLabel = ass3.Label;
            ass3.IsLabeled = true; //На этот оперетор мы переходим по условию 

            Assign ass4 = new Assign();
            ass4.Left = ass3.Result;
            ass4.Operation = OpCode.Plus;
            ass4.Right = new IntConst(1999);
            ass4.Result = new Var();

            IfGoto ifgt2 = new IfGoto();
            ifgt2.Condition = new IntConst(2);
            ifgt2.TargetLabel = ass2.Label;
            ass2.IsLabeled = true; //На этот оперетор мы переходим по условию 

            Assign ass5 = new Assign();
            ass5.Left = new IntConst(7);
            ass5.Operation = OpCode.Mul;
            ass5.Right = new IntConst(4);
            ass5.Result = new Var();

            Assign ass6 = new Assign();
            ass6.Left = new IntConst(100);
            ass6.Operation = OpCode.Div;
            ass6.Right = new IntConst(25);
            ass6.Result = new Var();

            ta_code.AddNode(ass1);
            ta_code.AddNode(ass2);
            ta_code.AddNode(ass3);
            ta_code.AddNode(ifgt1);
            ta_code.AddNode(ass4);
            ta_code.AddNode(ifgt2);
            ta_code.AddNode(ass5);
            ta_code.AddNode(ass6);

            string str_code = ta_code.ToString();
            Console.WriteLine(str_code);

            List<BasicBlock> b_blocks = BasicBlock.CreateBasicBlockList(ta_code);
            
            foreach (var bbl in b_blocks)
            {
                Console.WriteLine("Block[" + bbl.BlockId + "]");
                string bbl_code_str = bbl.CodeList.Aggregate("", (s, node) => s + node.ToString() + Environment.NewLine);
                Console.WriteLine(bbl_code_str);
            }

            /**Console.OutputEncoding = Encoding.UTF8;
            string FileName = @"..\..\sample.txt";
            try
            {
                string Text = File.ReadAllText(FileName);

                Scanner scanner = new Scanner();
                scanner.SetSource(Text, 0);

                var parser = new Parser.Parser(scanner);

                var b = parser.Parse();
                if (!b)
                    Console.WriteLine("Ошибка");
                else
                {
                    Console.WriteLine("Синтаксическое дерево построено");
                    //foreach (var st in parser.root.StList)
                    //Console.WriteLine(st);
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Файл {0} не найден", FileName);
            }
            catch (LexException e)
            {
                Console.WriteLine("Лексическая ошибка. " + e.Message);
            }
            catch (SyntaxException e)
            {
                Console.WriteLine("Синтаксическая ошибка. " + e.Message);
            }

            Console.ReadLine();**/
        }
    }
}
