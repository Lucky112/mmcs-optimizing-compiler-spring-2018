using System.Linq;
using Compiler.ThreeAddrCode.CFG;
using Compiler.ThreeAddrCode.Expressions;
using Compiler.ThreeAddrCode.Nodes;
using NUnit.Framework;

namespace Compiler.ThreeAddrCode.Tests
{
	[TestFixture]
	public class LiveVariableAnalysisTest
	{
		[Test]
		public void Test1()
		{
			var taCode = new TACode();
			var assgn1 = new Assign()
			{
				Left = null,
				Operation = OpCode.Copy,
				Right = new IntConst(91),
				Result = new Var("b")
			};
			var assgn2 = new Assign()
			{
				Left = null,
				Operation = OpCode.Copy,
				Right = assgn1.Result,
				Result = new Var("c")
			};
			var assgn3 = new Assign()
			{
				Left = assgn1.Result,
				Operation = OpCode.Plus,
				Right = new IntConst(10),
				Result = new Var("d")
			};
			assgn3.IsLabeled = true;

			var assgn4 = new Assign()
			{
				Left = null,
				Operation = OpCode.Copy,
				Right = new IntConst(2),
				Result = new Var("j")
			};
			var assgn5 = new Assign()
			{
				Left = null,
				Operation = OpCode.Copy,
				Right = new IntConst(0),
				Result = assgn3.Result
			};
			var assgn6 = new Assign()
			{
				Left = null,
				Operation = OpCode.Copy,
				Right = assgn2.Result,
				Result = assgn3.Result
			};
			assgn6.IsLabeled = true;

			taCode.AddNode(assgn1);
			taCode.AddNode(assgn2);
			taCode.AddNode(assgn3);
			taCode.AddNode(assgn4);
			taCode.AddNode(assgn5);
			taCode.AddNode(assgn6);

			var cfg = new ControlFlowGraph(taCode);
			IterativeAlgorithmAV ItAV = new IterativeAlgorithmAV(cfg);

			Assert.AreEqual(ItAV.IN.ElementAt(0).Value.Count, 0);
			Assert.AreEqual(ItAV.IN.ElementAt(1).Value.ElementAt(0), assgn1.Result);
			Assert.AreEqual(ItAV.IN.ElementAt(1).Value.ElementAt(1), assgn2.Result);
			Assert.AreEqual(ItAV.IN.ElementAt(2).Value.ElementAt(0), assgn2.Result);

			Assert.AreEqual(ItAV.OUT.ElementAt(0).Value.ElementAt(0), assgn1.Result);
			Assert.AreEqual(ItAV.OUT.ElementAt(0).Value.ElementAt(1), assgn2.Result);
			Assert.AreEqual(ItAV.OUT.ElementAt(1).Value.ElementAt(0), assgn2.Result);
			Assert.AreEqual(ItAV.OUT.ElementAt(2).Value.Count, 0);
		}
		[Test]
		public void Test2()
		{
			var taCode = new TACode();
			var assgn1 = new Assign()
			{
				Left = null,
				Operation = OpCode.Copy,
				Right = new IntConst(2),
				Result = new Var("a")
			};
			var assgn2 = new Assign()
			{
				Left = null,
				Operation = OpCode.Copy,
				Right = new IntConst(3),
				Result = new Var("b")
			};
			var assgn3 = new Assign()
			{
				Left = assgn1.Result,
				Operation = OpCode.Plus,
				Right = assgn2.Result,
				Result = new Var("c")
			};
			assgn3.IsLabeled = true;
			var assgn4 = new Assign()
			{
				Left = null,
				Operation = OpCode.Copy,
				Right = new IntConst(3),
				Result = assgn1.Result
			};
			var assgn5 = new Assign()
			{
				Left = null,
				Operation = OpCode.Copy,
				Right = new IntConst(4),
				Result = assgn2.Result
			};
			assgn4.IsLabeled = true;
			var assgn6 = new Assign()
			{
				Left = null,
				Operation = OpCode.Copy,
				Right = assgn1.Result,
				Result = assgn3.Result
			};
			assgn6.IsLabeled = true;

			taCode.AddNode(assgn1);
			taCode.AddNode(assgn2);
			taCode.AddNode(assgn3);
			taCode.AddNode(assgn4);
			taCode.AddNode(assgn5);
			taCode.AddNode(assgn6);

			var cfg = new ControlFlowGraph(taCode);
			IterativeAlgorithmAV ItAV = new IterativeAlgorithmAV(cfg);

			Assert.AreEqual(ItAV.IN.ElementAt(0).Value.Count, 0);
			Assert.AreEqual(ItAV.IN.ElementAt(1).Value.ElementAt(0), assgn1.Result);
			Assert.AreEqual(ItAV.IN.ElementAt(1).Value.ElementAt(1), assgn2.Result);
			Assert.AreEqual(ItAV.IN.ElementAt(2).Value.Count, 0);
			Assert.AreEqual(ItAV.IN.ElementAt(3).Value.ElementAt(0), assgn1.Result);

			Assert.AreEqual(ItAV.OUT.ElementAt(0).Value.ElementAt(0), assgn1.Result);
			Assert.AreEqual(ItAV.OUT.ElementAt(0).Value.ElementAt(1), assgn2.Result);
			Assert.AreEqual(ItAV.OUT.ElementAt(1).Value.Count, 0);
			Assert.AreEqual(ItAV.OUT.ElementAt(2).Value.ElementAt(1), assgn1.Result);
			Assert.AreEqual(ItAV.OUT.ElementAt(3).Value.Count, 0);
		}
	}
}
