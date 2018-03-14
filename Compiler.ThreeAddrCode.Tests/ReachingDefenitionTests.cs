using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using Compiler.ThreeAddrCode.Expressions;
using Compiler.ThreeAddrCode.Nodes;
using Compiler.ThreeAddrCode.CFG;
using Compiler.ThreeAddrCode.DFA.ReachingDefinitions;

namespace Compiler.ThreeAddrCode.Tests
{
    [TestFixture]
    class ReachingDefenitionTests
    {
        [Test]
        public void ReachingDefenitionTest1()
        {
            var taCode = new TACode();

            var ass1 = new Assign
            {
                Left = new IntConst(3),
                Operation = OpCode.Minus,
                Right = new IntConst(5),
                Result = new Var()
            };
            var ass2 = new Assign
            {
                Left = new IntConst(10),
                Operation = OpCode.Plus,
                Right = new IntConst(2),
                Result = new Var()
            };
            var ass3 = new Assign
            {
                Operation = OpCode.Minus,
                Right = new IntConst(1),
                Result = new Var()
            };
            var ifgt1 = new IfGoto
            {
                Condition = new IntConst(1),
                TargetLabel = ass3.Label
            };
            ass3.IsLabeled = true;
            var ass4 = new Assign
            {
                Left = ass3.Result,
                Operation = OpCode.Plus,
                Right = new IntConst(1999),
                Result = new Var()
            };
            var ifgt2 = new IfGoto
            {
                Condition = new IntConst(2),
                TargetLabel = ass2.Label
            };
            ass2.IsLabeled = true;
            var ass5 = new Assign
            {
                Left = new IntConst(7),
                Operation = OpCode.Mul,
                Right = new IntConst(4),
                Result = new Var()
            };
            var ass6 = new Assign
            {
                Left = new IntConst(100),
                Operation = OpCode.Div,
                Right = new IntConst(25),
                Result = new Var()
            };

            taCode.AddNode(ass1);
            taCode.AddNode(ass2);
            taCode.AddNode(ass3);
            taCode.AddNode(ifgt1);
            taCode.AddNode(ass4);
            taCode.AddNode(ifgt2);
            taCode.AddNode(ass5);
            taCode.AddNode(ass6);

            var cfg = new ControlFlowGraph(taCode);
            taCode.CreateBasicBlockList();
            var rd = new ReachingDefenition(taCode);

            Assert.AreEqual(5, rd.Gen.Count);
            Assert.AreEqual(5, rd.Kill.Count);
            Assert.AreEqual(8, rd.Gen.Values.Sum(x => x.Count));
            Assert.AreEqual(32, rd.Kill.Values.Sum(x => x.Count));
            var gen1 = new HashSet<Guid>() { ass1.Label };
            var kill1 = taCode.CodeList.ToList();
            kill1.RemoveAt(0);
            Assert.IsEmpty(rd.Gen.First().Value.Except(gen1));
            Assert.IsEmpty(rd.Kill.First().Value.Except(kill1.Select(x => x.Label)));
        }
    }
}
