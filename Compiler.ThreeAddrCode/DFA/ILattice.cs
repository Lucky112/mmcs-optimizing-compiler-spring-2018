using System;

namespace Compiler.ThreeAddrCode.DFA
{
    /// <summary>
    ///     Интерфейс элемента IN/OUT множества базового блока.
    ///     Представляет собой алгебраическую решетку
    ///     <a href="https://ru.wikipedia.org/wiki/Решётка_(алгебра)">Википедия</a>
    /// </summary>
    public interface ILattice: IEquatable<ILattice>
    {
    }
}
