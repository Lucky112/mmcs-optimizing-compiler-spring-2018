using System.Collections.Generic;
using Compiler.ThreeAddrCode.CFG;

namespace Compiler.ThreeAddrCode.DFA
{
    public interface ITransferFunction<T> where T: ILattice
    {
        ISet<T> Compute(BasicBlock block);
    }
}
