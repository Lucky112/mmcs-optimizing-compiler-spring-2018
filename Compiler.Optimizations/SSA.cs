using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.ThreeAddrCode.Nodes;
using Compiler.ThreeAddrCode.CFG;
using Compiler.ThreeAddrCode.DFA;

namespace Compiler.Optimizations
{
    public class SSA
    {
        public static void Apply(ControlFlowGraph cfg)
        {
            var reachingDefs = Analyze(cfg);

            foreach (var block in reachingDefs.Keys)
            {
                var innerDefs = reachingDefs[block].Item1.ToList();
                if (innerDefs.Count == 0)
                    continue;

                var groupedDefs = innerDefs.Select(def => cfg.Code.LabeledCode[def] as Assign).GroupBy(ass => ass.Result);

                foreach (var gr in groupedDefs)
                {
                    if (gr.Count() > 1)
                    {
                        var phi = new Phi()
                        {
                            Result = gr.Key,
                            Left = null,
                            Right = gr.Key,
                            Operation = ThreeAddrCode.OpCode.Phi,
                            DefenitionList = gr.ToList()
                        };
                        var firstOp = block.CodeList.FirstOrDefault();
                        if (firstOp != null)
                        {
                            cfg.Code.InsertNode(phi, firstOp.Label);
                            if (firstOp.IsLabeled)
                            {
                                phi.Label = firstOp.Label;
                                phi.IsLabeled = true;
                                firstOp.Label = Guid.NewGuid();
                                firstOp.IsLabeled = false;
                                ThreeAddrCode.TACodeNameManager.Instance.Label(firstOp.Label);
                            }
                        }
                    }
                }                
            }
        }

        public static InOutData<HashSet<Guid>> Analyze(ControlFlowGraph cfg)
        {
            var op = new ThreeAddrCode.DFA.ReachingDefinitions.Operations(cfg.Code);
            var tf = new ThreeAddrCode.DFA.ReachingDefinitions.TransferFunction(cfg.Code);
            var reachingDefs = new GenericIterativeAlgorithm<HashSet<Guid>>()
            {
                Finish = (a, b) =>
                {
                    var (a1, a2) = a;
                    var (b1, b2) = b;

                    return !a2.Except(b2).Any();
                },
                Comparer = (x, y) => !x.Except(y).Any(),
                Fill = () => (op.Lower, op.Lower)
            };
            return reachingDefs.Analyze(cfg,
                new ThreeAddrCode.DFA.ReachingDefinitions.Operations(cfg.Code),
                new ThreeAddrCode.DFA.ReachingDefinitions.TransferFunction(cfg.Code));
        }
    }
}
