using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.ThreeAddrCode.Nodes
{
    public class Phi : Assign
    {
        public Phi()
        {
            DefenitionList = new List<Assign>();
        }

        public List<Assign> DefenitionList { get; set; }

        public override string ToString()
        {
            return DefenitionList.Aggregate($"{TACodeNameManager.Instance[Label]} : {Result} = Ф(", (s, d) => s = s + $"{d.Result},") + ")";            
        }
    }
}
