using System;
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
        private readonly List<Node> _code;

        public BasicBlock(List<Node> code)
        {
            BlockId = _blockIdCounter++;

            _code = code;
        }

        public int BlockId { get; set; }

        public IEnumerable<Node> CodeList
        {
            get { return _code; }
        }

        public int GetBlockId()
        {
            return BlockId;
        }

        public static List<BasicBlock> CreateBasicBlockList(TACode code)
        {
            Debug.Assert(code != null);

            var basicBlockList = new List<BasicBlock>();

            var leaders = new List<int>();
            leaders.Add(0);

            var commands = code.CodeList.ToList();
            for (var i = 0; i < commands.Count; ++i)
            {
                var node = commands[i];
                if (node.IsLabeled)
                    leaders.Add(i);
                if (node is Goto)
                    leaders.Add(i + 1);
            }

            var ranges = leaders.Zip(leaders.Skip(1), Tuple.Create);
            foreach (var range in ranges)
            {
                var bbList = new List<Node>();

                for (var j = range.Item1; j < range.Item2; ++j)
                    bbList.Add(commands[j]);

                var bb = new BasicBlock(bbList);
                basicBlockList.Add(bb);
            }

            return basicBlockList;
        }
    }
}