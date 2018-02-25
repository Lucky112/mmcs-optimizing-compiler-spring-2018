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
        // Счетчик, инкрементируется каждый раз при создании блока и служит его номером
        private static int _blockIdCounter;

        // список программ в формате трехадресного кода
        private readonly List<Node> _code;

        /// <summary>
        /// Конструктор базового блока
        /// </summary>
        /// <param name="code">список узлов программы в трехадресной форме</param>
        public BasicBlock(List<Node> code)
        {
            BlockId = _blockIdCounter++;
            _code = code;
        }

        /// <summary>
        /// Идентификатор блока
        /// </summary>
        public int BlockId { get; private set; }

        /// <summary>
        /// Список узлов программы в трехадресной форме, связанных с блоков
        /// </summary>
        public IEnumerable<Node> CodeList
        {
            get { return _code; }
        }

        /// <summary>
        /// Построить список базовых блоков по программе в формате трехадресного кода
        /// </summary>
        /// <param name="code">программа в форме трехадресного кода</param>
        /// <returns>список базовых блоков</returns>
        public static List<BasicBlock> CreateBasicBlockList(TACode code)
        {
            Debug.Assert(code != null);
            
            var basicBlockList = new List<BasicBlock>();
            
            // список лидеров -- хранит "номера строк", 0 -- всегда лидер
            var leaders = new List<int> {0};

            var commands = code.CodeList.ToList();
            for (var i = 1; i < commands.Count; ++i)
            {
                var node = commands[i];

                // если в узел есть переход по GoTo
                if (node.IsLabeled)
                    leaders.Add(i);
                // если узел является переходом GoTo
                if (node is Goto)
                    leaders.Add(i + 1);
            }

            // группируем список как набор пар:
            // [a0, a1, a2, a3, ...] -> [(a0, a1), (a1, a2), (a2, a3), ...]
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