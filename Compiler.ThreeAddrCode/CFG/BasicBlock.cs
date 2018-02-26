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
    public class BasicBlock : IComparable<BasicBlock>
    {
        /// <summary>
        ///     Счетчик
        ///     <para>инкрементируется каждый раз при создании блока и служит его номером</para>
        /// </summary>
        private static int _blockIdCounter;

        /// <summary>
        ///     Конструктор базового блока
        /// </summary>
        /// <param name="codeList">список узлов программы в трехадресной форме</param>
        public BasicBlock(List<Node> codeList)
        {
            BlockId = _blockIdCounter++;
            CodeList = codeList;

            foreach (var node in codeList)
                node.Block = this;
        }

        /// <summary>
        ///     Идентификатор блока
        /// </summary>
        public int BlockId { get; }

        /// <summary>
        ///     Список узлов программы в трехадресной форме, связанных с блоком
        /// </summary>
        public List<Node> CodeList { get; }
            foreach (var l in leaders)
                Console.WriteLine(l);


            foreach (var r in ranges)
                Console.WriteLine(r.Item1 + ", " + r.Item2);

        public int CompareTo(BasicBlock other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return BlockId.CompareTo(other.BlockId);
        }
    }
}