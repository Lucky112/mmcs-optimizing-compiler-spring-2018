using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.ThreeAddrCode.Nodes;

namespace Compiler.ThreeAddrCode.DFA.ConstantPropagation
{
    public class Operations : ILatticeOperations<Dictionary<Guid, VarValue>>
    {
        private Dictionary<Guid, VarValue> upper;

        public Operations(TACode code)
        {
            upper = new Dictionary<Guid, VarValue>();
            foreach (var node in code.CodeList) {
                if (node is Assign) {
                    // т.к. node as Assign всегда Var верхние значения инициализированы как NAC
                    upper.Add(node.Label, new VarValue((node as Assign).Result));
                }
            }
        }

        public Dictionary<Guid, VarValue> Lower => new Dictionary<Guid, VarValue>();

        public Dictionary<Guid, VarValue> Upper => upper;

        public bool? Compare(Dictionary<Guid, VarValue> a, Dictionary<Guid, VarValue> b)
        {
            if (a.Count != b.Count)
                return null;
            if (a.Keys.Except(b.Keys).Any())
                return null;
            if (b.Keys.Except(a.Keys).Any())
                return null;
            int countLess = 0, countMore = 0;
            foreach (var key in a.Keys)
            {
                if (a[key] <= b[key])
                {
                    countLess++;
                }
                else {
                    countMore++;
                }
            }
            if (countLess == 0 || countMore == 0)
                return countLess > countMore;
            return null;
        }

        public Dictionary<Guid, VarValue> Operator(Dictionary<Guid, VarValue> a, Dictionary<Guid, VarValue> b) {
            Dictionary<Guid, VarValue> result = new Dictionary<Guid, VarValue>();
            foreach (var key in a.Keys)
            {
                result[key] = a[key].CollectionOperator(b[key]);
                if (b.ContainsKey(key))
                    b.Remove(key);
            }
            foreach (var key in b.Keys)
                result.Add(key, b[key]);
            return result;
        }
    }
}
