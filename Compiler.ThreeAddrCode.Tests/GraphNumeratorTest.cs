using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.ThreeAddrCode.Expressions;
using Compiler.ThreeAddrCode.Nodes;
using Compiler.ThreeAddrCode.CFG;
using Compiler.ThreeAddrCode.DFA.ReachingDefinitions;
using Compiler.ThreeAddrCode.DFA;
using Compiler.Parser;
using Compiler.Parser.Visitors;

namespace Compiler.ThreeAddrCode.Tests
{
    [TestFixture]
    class GraphNumeratorTest
    {
        [Test]
        public void GraphNumeratorTest0()
        {
            var code = @"a = 1;
                goto h;
                h: b = 1;
                goto h2;
                h2: c = 1;
                d = 1;";
            var scanner = new Scanner();
            scanner.SetSource(code, 0);
            var parser = new Parser.Parser(scanner);
            var b = parser.Parse();
            var astRoot = parser.root;
            var tacodeVisitor = new TACodeVisitor();
            astRoot.Visit(tacodeVisitor);
            var tacodeInstance = tacodeVisitor.Code;
            tacodeInstance.CreateBasicBlockList();
            var cfg = new ControlFlowGraph(tacodeInstance);
            var numer = GraphNumerator.BackOrder(cfg);
            Assert.AreEqual(0, numer.GetIndex(cfg.CFGNodes.ElementAt(0)));
            Assert.AreEqual(1, numer.GetIndex(cfg.CFGNodes.ElementAt(1)));
            Assert.AreEqual(2, numer.GetIndex(cfg.CFGNodes.ElementAt(2)));
        }
    }
}
