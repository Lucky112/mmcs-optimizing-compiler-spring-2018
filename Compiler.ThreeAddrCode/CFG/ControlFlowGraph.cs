using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.ThreeAddrCode.CFG
{
    public partial class ControlFlowGraph
    {
        public List<CFGNode> CfgNodes { get; private set; }

        public ControlFlowGraph(IEnumerable<BasicBlock> basicBlocks)
        {
            CfgNodes = new List<CFGNode>();
            CreateCFGNodes(basicBlocks);
        }

        /// <summary>
        /// Построить граф потока управления программы по заданным базовым блокам
        /// </summary>
        /// <param name="basicBlocks">базовые блоки программы</param>
        private void CreateCFGNodes(IEnumerable<BasicBlock> basicBlocks)
        {
            // TODO генерация блоков
        }

        /// <summary>
        /// Получить первый (входной) узел потока управления программы
        /// </summary>
        /// <returns>входной узел потока управления программы</returns>
        public CFGNode First()
        {
            return CfgNodes.First();
        }
    }
}
