﻿using System;
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

            cfg = new ControlFlowGraph(cfg.Code);
            reachingDefs = Analyze(cfg);
            RelaxPhi(cfg, reachingDefs);

            cfg = new ControlFlowGraph(cfg.Code);
            RenamePhiOccasions(cfg);

            cfg = new ControlFlowGraph(cfg.Code);
            ReducePhi(cfg);
        }

        private static void RenamePhiOccasions(ControlFlowGraph cfg)
        {
            var counter = cfg.Code.CodeList.Where(node => node is Phi).Cast<Phi>().GroupBy(phi => phi.Result).ToDictionary(gr => gr.Key, gr => 1);

            List<Guid> UsedDefs = new List<Guid>();
            
            dfs_visit(cfg.GetRoot(), new List<Guid>(), new Dictionary<Var, Var>(), counter);
        }

        private static void RelaxPhi(ControlFlowGraph cfg, Dictionary<BasicBlock, IEnumerable<Guid>> reachingDefs)
        {
            foreach (var block in reachingDefs.Keys)
            {
                if (block.Parents.Count() < 2)
                    continue;

                var innerDefs = reachingDefs[block].ToList();
                if (innerDefs.Count == 0)
                    continue;

                var groupedDefs = innerDefs.Select(def => cfg.Code.LabeledCode[def] as Assign).GroupBy(ass => ass.Result);

                foreach (var gr in groupedDefs)
                {
                    var phiNode = block.CodeList.ToList().Find(node => (node is Phi phi) && (phi.Result == gr.Key)) as Phi;
                    if (phiNode != null)                    
                        phiNode.DefenitionList = gr.ToList();                    
                }
            }
        }

        private static void ReducePhi(ControlFlowGraph cfg)
        {
            foreach (var block in cfg.CFGNodes)
            {
                foreach (var node in block.CodeList)
                {
                    if ((node is Phi phiNode) && (phiNode.DefenitionList.Count == 1))
                    {
                        var ass = new Assign()
                        {
                            Result = phiNode.Result,
                            Left = null,
                            Right = (phiNode.DefenitionList[0] as Assign).Result,
                            Operation = ThreeAddrCode.OpCode.Copy
                        };

                        cfg.Code.ReplaceNode(ass, phiNode);
                    }
                }
            }
        }
    

        private static void dfs_visit(BasicBlock curBlock, List<Guid> usedBlocks, Dictionary<Var, Var> varSubstitution, Dictionary<Var, int> counter)
        {
            usedBlocks.Add(curBlock.BlockId);

            foreach (var node in curBlock.CodeList)
                switch (node)
                {
                    case Phi phi:
                        var old = phi.Result;
                        phi.Result = new Var($"{phi.Result}_{counter[phi.Result]++}");
                        if (varSubstitution.ContainsKey(old))
                            varSubstitution[old] = phi.Result;
                        else
                            varSubstitution.Add(old, phi.Result);
                        break;

                    case Assign ass:
                        if (ass.Left is Var vL && varSubstitution.ContainsKey(vL))
                            ass.Left = varSubstitution[vL];
                        if (ass.Right is Var vR && varSubstitution.ContainsKey(vR))
                            ass.Right = varSubstitution[vR];
                        if (varSubstitution.ContainsKey(ass.Result))
                        {
                            old = ass.Result;
                            ass.Result = new Var($"{ass.Result}_{counter[ass.Result]++}");
                            varSubstitution[old] = ass.Result;
                        }
                        break;

                    case IfGoto ifgoto:
                        if (ifgoto.Condition is Var vC && varSubstitution.ContainsKey(vC))
                            ifgoto.Condition = varSubstitution[vC];
                        break;

                    case Print print:
                        if (print.Data is Var v && varSubstitution.ContainsKey(v))
                            print.Data = varSubstitution[v];
                        break;                    
                }

            foreach (var block in curBlock.Children)
                if (!usedBlocks.Contains(block.BlockId))
                    dfs_visit(block, usedBlocks, varSubstitution.ToDictionary(pair => pair.Key, pair => pair.Value), counter);
        }

        private static Dictionary<BasicBlock, IEnumerable<Guid>> Analyze(ControlFlowGraph cfg)
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
            var data = reachingDefs.Analyze(cfg,
                new ThreeAddrCode.DFA.ReachingDefinitions.Operations(cfg.Code),
                new ThreeAddrCode.DFA.ReachingDefinitions.TransferFunction(cfg.Code));

            return Verify(cfg, data);
        }

        private static Dictionary<BasicBlock, IEnumerable<Guid>> Verify(ControlFlowGraph cfg, InOutData<HashSet<Guid>> reachingDefs)
        {
            var result = new Dictionary<BasicBlock, IEnumerable<Guid>>();
            var defToRemove = new List<Guid>();
            foreach (var block in cfg.CFGNodes)
            {
                var parentOwnedDefs = block.Parents.Select(p => reachingDefs[p].Item2.Except(reachingDefs[p].Item1).GroupBy(id => (cfg.Code.LabeledCode[id] as Assign).Result));

                var correctDefs = parentOwnedDefs.SelectMany(p => p.Select(gr => gr.LastOrDefault()));
                defToRemove.AddRange(parentOwnedDefs.SelectMany(p => p.SelectMany(gr => gr)).Except(correctDefs));

                result.Add(block, reachingDefs[block].Item1.Where(id => !defToRemove.Contains(id)).ToList());
            }

            return result;
            //return reachingDefs.ToDictionary(pair => pair.Key, pair => pair.Value.Item1.Select(x => x));
        }

        private static void InjectPhi(ControlFlowGraph cfg, Dictionary<BasicBlock, IEnumerable<Guid>> reachingDefs)
        {
            foreach (var block in reachingDefs.Keys)
            {
                if (block.Parents.Count() < 2)
                    continue;

                var innerDefs = reachingDefs[block].ToList();
                if (innerDefs.Count == 0)
                    continue;

                //var test = innerDefs.Except(innerDefs.Where(def => cfg.Code.LabeledCode[def] is Assign)).Select(g => cfg.Code.LabeledCode[g]);
                var groupedDefs = innerDefs.Select(def => cfg.Code.LabeledCode[def] as Assign).GroupBy(ass => ass.Result);

                foreach (var gr in groupedDefs)
                {
                    if ((gr.Count() > 1) && IsInjectionNecessary(block, gr.Key))
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
                                cfg.Code.MoveLabel(firstOp, phi);                            
                        }
                    }
                }
            }
        }

        private static bool IsInjectionNecessary(BasicBlock block, Var v)
        {
            foreach (var node in block.CodeList)
                switch (node)
                {
                    case Assign ass:
                        if (ass.Left == v)
                            return true;
                        if (ass.Right == v)
                            return true;
                        if (ass.Result == v)
                            return false;
                        break;

                    case IfGoto ifgoto:
                        if (ifgoto.Condition == v)
                            return true;
                        break;

                    case Print print:
                        if (print.Data == v)
                            return true;
                        break;
                }

            return true;
        }


        private static void InjectPhi(ControlFlowGraph cfg)
        {
            foreach (var block in cfg.CFGNodes)
            {
                if (block.Parents.Count() < 2)
                    continue;

                var varList = new Dictionary<Guid, Var>();
                foreach (var node in block.CodeList)
                    switch (node)
                    {
                        case Assign ass:
                            if (ass.Left is Var vL && !varList.ContainsKey(vL.Id))
                                varList.Add(vL.Id, vL);
                            if (ass.Right is Var vR && !varList.ContainsKey(vR.Id))
                                varList.Add(vR.Id, vR);
                            if (ass.Result is Var res && !varList.ContainsKey(res.Id))
                                varList.Add(res.Id, res);
                            break;

                        case IfGoto ifgoto:
                            if (ifgoto.Condition is Var vC && !varList.ContainsKey(vC.Id))
                                varList.Add(vC.Id, vC);
                            break;

                        case Print print:
                            if (print.Data is Var v && !varList.ContainsKey(v.Id))
                                varList.Add(v.Id, v);
                            break;
                    }

                foreach (var v in varList.Values)
                {
                    var phi = new Phi()
                    {
                        Result = v,
                        Left = null,
                        Right = v,
                        Operation = ThreeAddrCode.OpCode.Phi
                    };
                    var firstOp = block.CodeList.FirstOrDefault();
                    if (firstOp != null)
                    {
                        cfg.Code.InsertNode(phi, firstOp.Label);
                        if (firstOp.IsLabeled)
                            cfg.Code.MoveLabel(firstOp, phi);
                    }
                }
            }
        }
    }
}