﻿using Compiler.ThreeAddrCode.Nodes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using QuickGraph;

namespace Compiler.ThreeAddrCode.CFG
{
    public class ControlFlowGraph
    {

        /// <summary>
        ///     Список узлов потока управления;
        ///     <para>Первый узел -- входной</para>
        /// </summary>
        public ReadOnlyCollection<BasicBlock> CFGNodes => _cfgNodes.AsReadOnly();

        public TACode Code { get; }

        private readonly List<BasicBlock> _cfgNodes;

        public BidirectionalGraph<BasicBlock, Edge<BasicBlock>> CFGAuxiliary =
            new BidirectionalGraph<BasicBlock, Edge<BasicBlock>>();

        public EdgeTypes EdgeTypes { get; set; }


        public void ReverseCFGNodes()
        {
            _cfgNodes.Reverse();
        }
        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="code">экземпляр программы в формате трехадресного кода</param>
        public ControlFlowGraph(TACode code)
        {
            Code = code;
            _cfgNodes = new List<BasicBlock>();

            CreateCFGNodes();
        }

        /// <summary>
        ///     Создать узлы графа потока управления программы
        /// </summary>
        private void CreateCFGNodes()
        {
            // оборачиваем ББ в CFG
            foreach (var block in Code.CreateBasicBlockList())
            {
                _cfgNodes.Add(block);
                CFGAuxiliary.AddVertex(block);
            }

            foreach (var cfgNode in _cfgNodes)
            {
                // блок содержит GoTo в последней строке
                if (cfgNode.CodeList.Last() is Goto gt)
                {
                    // ищем на какую строку идет переход
                    var targetFirst = Code.LabeledCode[gt.TargetLabel];

                    // забираем информацию о том, какому блоку принадлежит эта строка
                    var targetNode = _cfgNodes.First(n => n.Equals(targetFirst.Block));

                    // устанавливаем связи cfgNode <-> targetNode
                    cfgNode.AddChild(targetNode);
                    targetNode.AddParent(cfgNode);

                    CFGAuxiliary.AddEdge(new Edge<BasicBlock>(cfgNode, targetNode));
                }
            }

            // каждый блок является родителем последующего
            var nodeList = CFGNodes.ToList();
            for (int i = 0; i < nodeList.Count - 1; ++i)
            {
                var cur = nodeList[i];
                var next = nodeList[i + 1];

                // если последняя строчка -- чистый goto (не if), то дуги быть не может
                if (cur.CodeList.Last().GetType() == typeof(Goto)) continue;

                cur.AddChild(next);
                next.AddParent(cur);

                CFGAuxiliary.AddEdge(new Edge<BasicBlock>(cur, next));
            }

            EdgeTypes = new EdgeTypes();
            //ClassificateEdges();
        }

        public void ClassificateEdges()
        {
            var depthTree = new DepthSpanningTree(this);
            foreach (var edge in CFGAuxiliary.Edges)
            {
                if (depthTree.SpanningTree.Edges.Any(e => e.Target.Equals(edge.Target) && e.Source.Equals(edge.Source)))
                {
                    EdgeTypes.Add(edge, EdgeType.Coming);
                }
                else if (depthTree.FindBackwardPath(edge.Source, edge.Target))
                {
                    EdgeTypes.Add(edge, EdgeType.Retreating);
                }
                else
                {
                    EdgeTypes.Add(edge, EdgeType.Cross);
                }
            }
        }

        public int NumberOfVertices()
        {
            return CFGNodes.Count;
        }

        public BasicBlock GetRoot()
        {
            return (NumberOfVertices() > 0) ? CFGNodes.ElementAt(0) : null;
        }

        /// <summary>
        /// Возвращает true, если CFG приводим, иначе - false
        /// </summary>
        public bool IsReducible { get => isReducible(); }

        /// <summary>
        /// Вычисление глубины CFG
        /// </summary>
        /// <returns>
        /// Возвращает глубину CFG
        /// </returns>
        public int GetDepth() {
            // Нужно убрать отсюда и добавить в вызов конструктора
            if (EdgeTypes == null)
                ClassificateEdges();

            List<BasicBlock> visitedNodes = new List<BasicBlock>();
            return GetDepthRecursive(GetRoot(), 0, ref visitedNodes);
        }

        /// <summary>
        /// Вычисление глубины CFG
        /// </summary>
        /// <param name="root">текущий узел</param>
        /// <param name="depth">текущая максимальная глубина</param>
        /// <param name="visitedNodes">список посещенных вершин</param>
        /// <returns>
        /// Возвращает глубину CFG
        /// </returns>
        private int GetDepthRecursive(BasicBlock root, int depth, ref List<BasicBlock> visitedNodes) {
            if (
                root.Children.Count() == 0 ||
                visitedNodes.Contains(root)
            ) return depth;

            visitedNodes.Add(root);

            foreach (var children in root.Children) {
                if (visitedNodes.Contains(children)) continue;

                if (EdgeTypes.First(edge =>
                                    edge.Key.Source.BlockId == root.BlockId &&
                                    edge.Key.Target.BlockId == children.BlockId)
                             .Value == EdgeType.Retreating)
                {
                    depth = GetDepthRecursive(children, depth + 1, ref visitedNodes);
                }
                else
                {
                    depth = GetDepthRecursive(children, depth, ref visitedNodes);
                }
            }

            return depth;
        }

        /// <summary>
        /// Возвращает глубину (by rphaet0n)
        /// </summary>
        public uint Depth { get => getDepth(); }

        private List<BasicBlock> visitedNodes;

        private BasicBlock getCFGEntry()
        {
            return _cfgNodes[0];
        }

        private uint getDepth()
        {
            if (EdgeTypes == null)
                ClassificateEdges();
            visitedNodes = new List<BasicBlock>();
            var cfgEntry = getCFGEntry();
            return CalcCFGDepth(cfgEntry);
        }

        private uint CalcCFGDepth(BasicBlock cfgEntry)
        {
            visitedNodes.Add(cfgEntry);
            var childrenDepths = new List<uint>();
            foreach (var child in cfgEntry.Children)
            {
                if (!visitedNodes.Contains(child))
                {
                    if (EdgeTypes.First(edge => edge.Key.Source.BlockId == cfgEntry.BlockId &&
                                                edge.Key.Target.BlockId == child.BlockId)
                        .Value == EdgeType.Retreating)
                        childrenDepths.Add(1 + CalcCFGDepth(child));
                    else
                        childrenDepths.Add(CalcCFGDepth(child));
                }
            }
            visitedNodes.Remove(cfgEntry);
            return childrenDepths.Count > 0 ? childrenDepths.Max() : 0;
        }

        /// <summary>
    /// Проверка CFG на приводимость
    /// </summary>
    /// <returns>Возвращает true, если CFG приводим, иначе - false</returns>
        private bool isReducible()
            {
                //Если ребра не классифицированы
                if (EdgeTypes.Count == 0)
                    this.ClassificateEdges();
                //Отбираем все обратные дуги
                var retreatiangEdges = EdgeTypes.Where(elem => elem.Value == EdgeType.Retreating).Select(pair => pair.Key);
                var count = retreatiangEdges.Count();
                //Если таковых нет - CFG приводим
                if (retreatiangEdges.Count() == 0)
                    return true;
                //Строим дерево доминанто
                var domMatrix = new DominatorTree(this).Matrix;
                //Проверяем каждую обратную дугу на обратимость(target доминирует на source в DominatorTree)
                foreach (var edge in retreatiangEdges)
                {
                    //DominatorTree.matrix(i, j).HasLine <=> j dom i. (c) Max
                    var rowWithSource = domMatrix.First(row => row.BasicBlock.BlockId == edge.Source.BlockId);
                    var edgeInDomTree = rowWithSource.ItemDoms.First(cell => cell.BasicBlock.BlockId == edge.Target.BlockId);
                    if (!edgeInDomTree.HasLine)
                        return false;
                }
                return true;
            }
        }
}