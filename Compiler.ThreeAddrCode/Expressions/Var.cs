using System;

namespace Compiler.ThreeAddrCode.Expressions
{
    /// <summary>
    ///     Операнд-переменная
    /// </summary>
    public class Var : Expr
    {
        public Var()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public override string ToString()
        {
            return Id.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is Var)
                return Id == (obj as Var).Id;
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}