using System;
using Compiler.ThreeAddrCode.CFG;

namespace Compiler.ThreeAddrCode.DFA
{
    /// <summary>
    /// Статический класс анализатора графа потока управления
    /// </summary>
    public class Analyzer
    {
        private Analyzer()
        {
        }

        /// <summary>
        ///     Выполнить анализ графа потока управления указанной стратегией и получить копию графа
        ///     с заполненными IN/OUT множествами
        /// </summary>
        /// <param name="cfg">граф потока управления</param>
        /// <param name="strategy">стратегия обхода графа потока управления</param>
        /// <returns>копия графа с заполненными IN/OUT множествами</returns>
        public static ControlFlowGraph Analyze(ControlFlowGraph cfg, IStrategy<Guid> strategy)
        {
            throw new NotImplementedException();
        }
    }
}