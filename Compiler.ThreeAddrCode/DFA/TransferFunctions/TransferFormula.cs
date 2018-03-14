using System;
using System.Collections.Generic;
using Compiler.ThreeAddrCode.CFG;

namespace Compiler.ThreeAddrCode.DFA.TransferFunctions
{
    class TransferFormula : ITransferFunction<Guid>
    {
        public IStrategy<Guid> Strategy { get; }

        public TransferFormula(IStrategy<Guid> strategy)
        {
            Strategy = strategy;
        }

        public ISet<Guid> Compute(BasicBlock block)
        {
            var gen = Strategy.Gen[block.BlockId];
            var kill = Strategy.Kill[block.BlockId];
            var inset = new HashSet<Guid>(block.In);
            inset.ExceptWith(kill);
            inset.UnionWith(gen);
            return inset;
        }
    }
}
