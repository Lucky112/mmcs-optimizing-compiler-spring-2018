using System;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.ThreeAddrCode.DFA.ReachingDefinitions
{
    public class ReachingDefenition : IStrategy<Guid>
    {
        Dictionary<Guid, HashSet<Guid>> gen = new Dictionary<Guid, HashSet<Guid>>();
        Dictionary<Guid, HashSet<Guid>> kill = new Dictionary<Guid, HashSet<Guid>>();

        public Dictionary<Guid, HashSet<Guid>> Gen => gen;
        public Dictionary<Guid, HashSet<Guid>> Kill => kill;

        public ReachingDefenition(TACode code)
        {
            var allLabels = new HashSet<Guid>(code.CodeList.Select(x => x.Label));
            foreach (var node in code.CodeList)
            {
                if (gen.ContainsKey(node.Block.BlockId))
                    gen[node.Block.BlockId].Add(node.Label);
                else
                    gen.Add(node.Block.BlockId, new HashSet<Guid>() { node.Label });
            }
            foreach (var pair in gen)
            {
                var killed = new HashSet<Guid>(allLabels);
                killed.ExceptWith(pair.Value);
                kill.Add(pair.Key, killed);
            }
        }
    }
}
