using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.ThreeAddrCode.CFG
{
    public partial class ControlFlowGraph
    {
        private List<BasicBlock> _basicBlocks;

        public ControlFlowGraph(IOrderedEnumerable<BasicBlock> basicBlocks)
        {
            _basicBlocks = basicBlocks.ToList();

        }
    }
}
