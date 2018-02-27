namespace Compiler.ThreeAddrCode
{
    public enum OpCode
    {
        Plus,
        Minus,
        Mul,
        Div,

        UnaryMinus,
        Copy,
        
        Greater,
        Less,
        GreaterEq,
        LessEq,
        Equal,
        NotEqual,
        Not
    }
}