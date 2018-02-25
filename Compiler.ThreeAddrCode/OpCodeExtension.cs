namespace Compiler.ThreeAddrCode
{
    public static class OpCodeExtension
    {
        public static string GetSymbol(this OpCode op)
        {
            switch (op)
            {
                case OpCode.Plus: return "+";
                case OpCode.UnaryMinus:
                case OpCode.Minus: return "-";
                case OpCode.Mul: return "*";
                case OpCode.Div: return "/";
                case OpCode.Copy: return "";

                default: return "unknown";
            }
        }
    }
}