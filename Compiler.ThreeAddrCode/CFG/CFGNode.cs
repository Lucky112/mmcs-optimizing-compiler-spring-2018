using System;
using System.Collections.Generic;

namespace Compiler.ThreeAddrCode.CFG
{
    /// <summary>
    /// Класс-обёртка, который содержит в себе экземпляр базового блока и списки его детей и потомков
    /// </summary>
    public class CFGNode
    {
        /// <summary>
        /// Базовый блок
        /// </summary>
        private BasicBlock bbl;

        /// <summary>
        /// Дети узла CFG графа
        /// </summary>
        private List<BasicBlock> childs;
        
        /// <summary>
        /// Родители узла CFG графа 
        /// </summary>
        private List<BasicBlock> parents;

        /// <summary>
        /// Конструктор узла графа CFG
        /// </summary>
        /// <param name="bbl"></param>
        public CFGNode(BasicBlock bbl)
        {
            this.bbl = bbl;
            childs = new List<BasicBlock>();
            parents = new List<BasicBlock>();
        }

        public List<BasicBlock> GetChilds()
        {
            return this.childs;           
        }

        public List<BasicBlock> GetParents()
        {
            return this.parents;
        }

        /// <summary>
        /// Новый ребёнок
        /// </summary>
        /// <param name="child"></param>
        public void AddChild(BasicBlock child)
        {
            this.childs.Add(child);
        }

        /// <summary>
        /// Новый родитель
        /// </summary>
        /// <param name="parent"></param>
        public void AddParent(BasicBlock parent)
        {
            this.parents.Add(parent);
        }
    }
}
