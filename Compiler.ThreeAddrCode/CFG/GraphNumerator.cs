using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.ThreeAddrCode.CFG
{
    public class GraphNumerator : IGraphNumerator
    {
        public static GraphNumerator BackOrder(ControlFlowGraph graph)
        {
            var root = graph.CFGNodes.ElementAt(0);
            var num = new GraphNumerator();
            var index = 0;

            void Iter(BasicBlock node)
            {
                var children = node.Children;
                foreach(var c in children)
                    Iter(node);
                num._num[node] = index;
                index++;
            }

            return num;
        }

        private readonly Dictionary<BasicBlock, int> _num = new Dictionary<BasicBlock, int>();

        public int? GetIndex(BasicBlock b) 
            => _num.TryGetValue(b, out var res) ?
                new int?(res) : null;
    }

    public interface IGraphNumerator
    {
        int? GetIndex(BasicBlock b);
    }
}
