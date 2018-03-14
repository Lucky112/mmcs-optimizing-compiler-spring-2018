using System.Collections.Generic;
using Compiler.ThreeAddrCode.CFG;

namespace Compiler.ThreeAddrCode.DFA
{
    public interface ITransferFunction<T>
    {
        ISet<T> Compute(BasicBlock block);
    }
}
