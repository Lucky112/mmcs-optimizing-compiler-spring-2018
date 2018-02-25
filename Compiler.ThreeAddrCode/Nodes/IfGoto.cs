using Compiler.ThreeAddrCode.Expressions;

namespace Compiler.ThreeAddrCode.Nodes
{
    /// <summary>
    ///     Оператор условного перехода
    /// </summary>
    public class IfGoto : Goto
    {
        /// <summary>
        ///     Условие перехода
        /// </summary>
        public Expr Condition { get; set; }

        public override string ToString()
        {
            return string.Format("{0} : if {1} goto {2}", Label, Condition, TargetLabel);
        }
    }
}