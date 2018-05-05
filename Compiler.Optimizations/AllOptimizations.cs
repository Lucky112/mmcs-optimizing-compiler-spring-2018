using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.Optimizations;
using Compiler.ThreeAddrCode;
using Compiler.ThreeAddrCode.Nodes;

namespace Compiler.Optimizations
{
	public class AllOptimizations
	{
		private List<IOptimization> BasicBlockOptimizationList()
		{
			List<IOptimization> optimizations = new List<IOptimization>();

			AlgebraicOptimization algOptimization = new AlgebraicOptimization();
			CopyPropagation copyPropOptimization = new CopyPropagation();
			ConstantFolding constFoldingOptimization = new ConstantFolding();
			ConstantPropagation constPropOptimization = new ConstantPropagation();
			DeclarationOptimization declOptimization = new DeclarationOptimization();

			optimizations.Add(copyPropOptimization);
			optimizations.Add(constFoldingOptimization);
			optimizations.Add(constPropOptimization);
			//optimizations.Add(declOptimization);
            optimizations.Add(algOptimization);

            return optimizations;
		}

        private List<IOptimization> O2OptimizationList()
        {
            return new List<IOptimization>();
        }


        public TACode ApplyAllOptimizations(TACode code)
		{
			List<IOptimization> o1Optimizations = BasicBlockOptimizationList();
            var canApplyAny = true;

            while (canApplyAny)
            {
                canApplyAny = false;
                var blocks = code.CreateBasicBlockList().ToList();
                var codeList = new List<Node>();

                foreach (var b in blocks)
                {
                    var block = b.CodeList.ToList();
                    for (int i = 0; i < o1Optimizations.Count; i++)
                    {
                        block = o1Optimizations[i].Optimize(block, out var applied);
                        canApplyAny = canApplyAny || applied;
                    }
                    codeList.AddRange(block);
                }

                code = new TACode();
                code.CodeList = codeList;
            }

			return code;
		}
	}
}
