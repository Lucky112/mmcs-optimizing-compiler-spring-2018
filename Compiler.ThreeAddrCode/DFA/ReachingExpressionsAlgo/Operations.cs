using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.ThreeAddrCode.Nodes;

namespace Compiler.ThreeAddrCode.DFA.ReachingExpressionsAlgo
{
    //Реализация методов интерфейса операторов полурешётки для достигающих выражений
    //Реализация взята с ReachingDefinitions с небольшими изменениями
    public class Operations : ILatticeOperations<HashSet<Guid>>
    {
        /// <summary>
        /// Верхняя граница полурешётки для достигающих выражений. 
        /// Верхняя граница - все выражения трёх-адрессного кода 
        /// </summary>
        private HashSet<Guid> upper; 

        public Operations(TACode code)
        {
            foreach (var node in code.CodeList)
            {
                if (node is Assign ass) //Если узел-присваивание, то добавляем его в верхнюю границу
                    upper.Add(ass.Label);
            }
        }

        /// <summary>
        /// Нижняя граница полурешётки для достигающих выражений
        /// Нижняя граница - пустое множество
        /// </summary>
        public HashSet<Guid> Lower => new HashSet<Guid>();

        public HashSet<Guid> Upper => upper;

        public bool? Compare(HashSet<Guid> a, HashSet<Guid> b)
        {
            var symmDiff = a.Except(b).Union(b.Except(a));
            if (symmDiff.Count() == 0)
                return null;
            return b.IsSubsetOf(a);
        }

        /// <summary>
        /// Оператор сбора. Для достигающих выражений - пересечение
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public HashSet<Guid> Operator(HashSet<Guid> a, HashSet<Guid> b) =>
            new HashSet<Guid>(a.Intersect(b));
    }
}
