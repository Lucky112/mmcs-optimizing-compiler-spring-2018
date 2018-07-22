using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.ThreeAddrCode.Nodes
{
    /// <summary>
    /// Оператор Phi для построения  SSA-формы
    /// Является определением, поэтому наследуется от оператора присваивания
    /// </summary>
    public class Phi : Assign
    {
        public Phi()
        {
            DefenitionList = new List<Assign>();
            DefinitionPathes = new Dictionary<Assign, CFG.BasicBlock>();
        }

        public List<Assign> DefenitionList { get; set; }
        public Dictionary<Assign, CFG.BasicBlock> DefinitionPathes { get; set;}

        public override string ToString()
        {
            return DefenitionList.Aggregate($"{TACodeNameManager.Instance[Label]} : {Result} = Я(", (s, d) => s = s + $"{d.Result}, ") + ")";
        }
    }
}

//u03C6
