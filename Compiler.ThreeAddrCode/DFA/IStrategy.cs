using System;
using System.Collections.Generic;

namespace Compiler.ThreeAddrCode.DFA
{
    /// <summary>
    ///     Интерфейс стратегии обхода графа потока управления
    /// </summary>
    public interface IStrategy<T>
    {
        Dictionary<Guid, HashSet<T>> Gen { get; }
        Dictionary<Guid, HashSet<T>> Kill { get; }
    }
}
