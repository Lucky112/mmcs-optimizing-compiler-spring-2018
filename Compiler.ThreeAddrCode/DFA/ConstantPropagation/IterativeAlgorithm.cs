using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.ThreeAddrCode.CFG;

namespace Compiler.ThreeAddrCode.DFA.ConstantPropagation
{
    public class IterativeAlgorithm : IAlgorithm<Dictionary<Guid, VarValue>>
    {
        public InOutData<Dictionary<Guid, VarValue>> Analyze(ControlFlowGraph graph, ILatticeOperations<Dictionary<Guid, VarValue>> ops, ITransferFunction<Dictionary<Guid, VarValue>> f)
        {
            return null;
        }
    }
}
