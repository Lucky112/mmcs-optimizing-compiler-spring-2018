using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.ThreeAddrCode.Nodes;
using Compiler.ThreeAddrCode.CFG;
using Compiler.ThreeAddrCode.DFA;
using Compiler.ThreeAddrCode.Expressions;

namespace Compiler.Optimizations
{
    public class SSA
    {
        public static void Apply(ControlFlowGraph cfg)
        {
            var reachingDefs = Analyze(cfg);
            InjectPhi(cfg, reachingDefs);
            RenamePhiOccasions(new ControlFlowGraph(cfg.Code));
        }

        private static void RenamePhiOccasions(ControlFlowGraph cfg)
        {
            var counter = cfg.Code.CodeList.Where(node => node is Phi).Cast<Phi>().GroupBy(phi => phi.Result).ToDictionary(gr => gr.Key, gr => 0);

            List<Guid> UsedDefs = new List<Guid>();
            
            foreach (var node in cfg.Code.CodeList)
                if (node is Phi phi)
                {
                    foreach (var def in phi.DefenitionList)
                        if (!UsedDefs.Contains(def.Label))
                        {
                            def.Result = new ThreeAddrCode.Expressions.Var($"{def.Result}_{counter[phi.Result]++}");
                            UsedDefs.Add(def.Label);
                        }
                }

            dfs_visit(cfg.GetRoot(), new List<Guid>(), null, null, counter);
        }

        private static void dfs_visit(BasicBlock curBlock, List<Guid> usedBlocks, ThreeAddrCode.Expressions.Var oldVar, ThreeAddrCode.Expressions.Var newVar, Dictionary<Var, int> counter)
        {
            usedBlocks.Add(curBlock.BlockId);

            foreach (var node in curBlock.CodeList)
                switch (node)
                {
                    case Phi phi:
                        oldVar = phi.Result;
                        phi.Result = new Var($"{phi.Result}_{counter[phi.Result]++}");
                        newVar = phi.Result;
                        break;

                    case Assign ass:
                        if (ass.Left == oldVar)
                            ass.Left = newVar;
                        if (ass.Right == oldVar)
                            ass.Right = newVar;
                        break;

                    case IfGoto ifgoto:
                        if (ifgoto.Condition == oldVar)
                            ifgoto.Condition = newVar;
                        break;

                    case Print print:
                        if (print.Data == oldVar)
                            print.Data = newVar;
                        break;                    
                }

            foreach (var block in curBlock.Children)
                if (!usedBlocks.Contains(block.BlockId))
                    dfs_visit(block, usedBlocks, oldVar, newVar, counter);
        }

        private static InOutData<HashSet<Guid>> Analyze(ControlFlowGraph cfg)
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

        private static void InjectPhi(ControlFlowGraph cfg, InOutData<HashSet<Guid>> reachingDefs)
        {
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
    }
}
