using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Compiler.ThreeAddrCode.Nodes;

namespace Compiler.ThreeAddrCode.CFG
{
    /// <summary>
    ///     Базовый блок участка программы
    /// </summary>
    public class BasicBlock
    {
        // Счетчик инкрементируется каждый раз при создании блока
        private static int _blockIdCounter;

        // список программ в формате трехадресного кода
        private readonly List<TACode> _code;

        public BasicBlock()
        {
            BlockId = _blockIdCounter++;

            _code = new List<TACode>();
        }

        public BasicBlock(BasicBlock block)
        {
            BlockId = block.BlockId;
            _code = block._code;
        }

        public int BlockId { get; set; }

        public IEnumerable<TACode> CodeList
        {
            get { return _code; }
        }

        public int GetBlockId()
        {
            return BlockId;
        }

        public static List<BasicBlock> CreateBasicBlockList(List<TACode> code)
        {
            Debug.Assert(code != null);
            var basicBlockList = new List<BasicBlock>();
            var leaderNumb = new List<int>();
            var beforeLeadearNumb = new List<int>();

            leaderNumb.Add(0);
            for (var i = 1; i < code.Count; i++)
            {
                var node = code[i].CodeList.ElementAt(i);
                if (node.IsLabeled)
                {
                    leaderNumb.Add(i);
                    beforeLeadearNumb.Add(i - 1);
                }
                if ((node is Goto) && i < code.Count - 1)
                {
                    beforeLeadearNumb.Add(i);
                    leaderNumb.Add(i + 1);
                }
            }
            beforeLeadearNumb.Add(code.Count - 1);
            var j = 0;
            for (var i = 0; i < leaderNumb.Count; i++)
            {
                basicBlockList[i] = new BasicBlock();
                var currentPosition = leaderNumb[i];
                while (currentPosition <= beforeLeadearNumb[i])
                {
                    basicBlockList[i]._code.Add(code[j]);
                    currentPosition = currentPosition + 1;
                    j = j + 1;
                }
            }

            return basicBlockList;
        }
    }
}