namespace Compiler.ThreeAddrCode.Expressions
{
    /// <summary>
    ///     Операнд-константа (числа типа int)
    /// </summary>
    public class IntConst : Expr
    {
        public IntConst(int num)
        {
            Num = num;
        }

        public int Num { get; set; }

        public override string ToString()
        {
            return Num.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is IntConst)
                return Num == (obj as IntConst).Num;
            return false;
        }

        public override int GetHashCode()
        {
            return Num.GetHashCode();
        }
    }
}