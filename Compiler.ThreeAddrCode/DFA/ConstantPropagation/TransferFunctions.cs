using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.ThreeAddrCode.CFG;
using Compiler.ThreeAddrCode.Nodes;
using Compiler.ThreeAddrCode.Expressions;

namespace Compiler.ThreeAddrCode.DFA.ConstantPropagation
{
    public class TransferFunctions : ITransferFunction<Dictionary<Guid, VarValue>>
    {
        public Dictionary<Guid, VarValue> Transfer(BasicBlock basicBlock, Dictionary<Guid, VarValue> input, ILatticeOperations<Dictionary<Guid, VarValue>> ops)
        {
            Dictionary<Guid, VarValue> res = input;
            foreach (var node in basicBlock.CodeList)
            {
                if (node is Assign)
                {
                    var asNode = node as Assign;
                    if (asNode.Left == null && asNode.Right is IntConst) {
                        res[node.Label] = new VarValue(asNode.Right as IntConst);
                    }
                    if (asNode.Left != null && asNode.Right != null) {
                        if (res[(asNode.Left as Var).Id].varType == VarValue.Type.CONST &&
                            res[(asNode.Right as Var).Id].varType == VarValue.Type.CONST)

                            res[node.Label] = res[(asNode.Left as Var).Id].UseOperation(res[(asNode.Right as Var).Id], asNode.Operation);

                        else if (res[(asNode.Left as Var).Id].varType == VarValue.Type.NAC ||
                            res[(asNode.Right as Var).Id].varType == VarValue.Type.NAC)

                            res[node.Label] = new VarValue(asNode.Result);

                        else res[node.Label] = new VarValue();
                    }
                }
            }
            return res;
        }
    }
}
