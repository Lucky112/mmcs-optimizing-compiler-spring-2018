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
        //Пример из презентации
        [Test]
        public void ReachingDefenitionTest0()
        {
            var taCode = new TACode();

            var ass1 = new Assign
            {
                Left = new IntConst(1000),
                Operation = OpCode.Minus,
                Right = new IntConst(5),
                Result = new Var()
            };
            var ass2 = new Assign
            {
                Left = new IntConst(2000),
                Operation = OpCode.Copy,
                Result = new Var()
            };
            var ass3 = new Assign
            {
                Operation = OpCode.Copy,
                Right = new IntConst(3000),
                Result = new Var()
            };
            var ass4 = new Assign
            {
                Left = ass1.Result,
                Operation = OpCode.Plus,
                Right = new IntConst(1),
                Result = ass1.Result
            };
            ass4.IsLabeled = true;
            var ass5 = new Assign
            {
                Left = ass2.Result,
                Operation = OpCode.Minus,
                Right = new IntConst(4),
                Result = ass2.Result
            };
            var ass6 = new Assign
            {
                Operation = OpCode.Copy,
                Right = new IntConst(4000),
                Result = ass3.Result
            };
            var ass7 = new Assign
            {
                Operation = OpCode.Copy,
                Right = new IntConst(5000),
                Result = ass1.Result
            };
            ass7.IsLabeled = true;
            var ifgt1 = new IfGoto
            {
                Condition = new IntConst(1),
                TargetLabel = ass7.Label
            };
            var ifgt2 = new IfGoto
            {
                Condition = new IntConst(2),
                TargetLabel = ass4.Label
            };
            var empty = new Empty { };
            {
                taCode.AddNode(ass1);
                taCode.AddNode(ass2);
                taCode.AddNode(ass3);
                taCode.AddNode(ass4);
                taCode.AddNode(ass5);
                taCode.AddNode(ifgt1);
                taCode.AddNode(ass6);
                taCode.AddNode(ass7);
                taCode.AddNode(ifgt2);
                taCode.AddNode(empty);
            }
            var cfg = new ControlFlowGraph(taCode);
            taCode.CreateBasicBlockList();

            var op = new Operations(taCode);
            var tf = new TransferFunction();


            var (gen, kill) = tf.GetGenAndKill(cfg.CFGNodes.ElementAt(0), op);
            Assert.True(gen.SetEquals(new HashSet<Guid> { ass1.Label, ass2.Label, ass3.Label }));
            Assert.True(kill.SetEquals(new HashSet<Guid> { ass4.Label, ass5.Label, ass6.Label, ass7.Label }));

            (gen, kill) = tf.GetGenAndKill(cfg.CFGNodes.ElementAt(1), op);
            Assert.True(gen.SetEquals(new HashSet<Guid> { ass4.Label, ass5.Label }));
            Assert.True(kill.SetEquals(new HashSet<Guid> { ass1.Label, ass2.Label, ass7.Label }));

            (gen, kill) = tf.GetGenAndKill(cfg.CFGNodes.ElementAt(2), op);
            Assert.True(gen.SetEquals(new HashSet<Guid> { ass6.Label }));
            Assert.True(kill.SetEquals(new HashSet<Guid> { ass3.Label }));

            (gen, kill) = tf.GetGenAndKill(cfg.CFGNodes.ElementAt(3), op);
            Assert.True(gen.SetEquals(new HashSet<Guid> { ass7.Label }));
            Assert.True(kill.SetEquals(new HashSet<Guid> { ass1.Label, ass4.Label }));



            var inout = (new IterativeAlgorithm()).Analyze(cfg, op, tf);

            var trueInOut = new DFA.InOutData<HashSet<System.Guid>>();
            trueInOut.Add(cfg.CFGNodes.ElementAt(0),
                (new HashSet<Guid>(),
                 new HashSet<Guid> { ass1.Label, ass2.Label, ass3.Label })
            );
            trueInOut.Add(cfg.CFGNodes.ElementAt(1),
                (new HashSet<Guid> { ass1.Label, ass2.Label, ass3.Label, ass5.Label, ass6.Label, ass7.Label },
                new HashSet<Guid> { ass3.Label, ass4.Label, ass5.Label, ass6.Label })
            );
            trueInOut.Add(cfg.CFGNodes.ElementAt(2),
                (new HashSet<Guid> { ass3.Label, ass4.Label, ass5.Label, ass6.Label },
                new HashSet<Guid> { ass4.Label, ass5.Label, ass6.Label })
            );
            trueInOut.Add(cfg.CFGNodes.ElementAt(3),
                (new HashSet<Guid> { ass3.Label, ass4.Label, ass5.Label, ass6.Label },
                new HashSet<Guid> { ass3.Label, ass5.Label, ass6.Label, ass7.Label })
            );
            trueInOut.Add(cfg.CFGNodes.ElementAt(4),
                (new HashSet<Guid> { ass3.Label, ass5.Label, ass6.Label, ass7.Label },
                new HashSet<Guid> { ass3.Label, ass5.Label, ass6.Label, ass7.Label })
            );

            foreach (var x in cfg.CFGNodes)
                Assert.True(trueInOut[x].Item1.SetEquals(inout[x].Item1) && trueInOut[x].Item2.SetEquals(inout[x].Item2));
        }

        [Test] //Рандомный пример который я дороботал
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

            var op = new Operations(taCode);
            var tf = new TransferFunction();
            

            var (gen, kill) = tf.GetGenAndKill(cfg.CFGNodes.ElementAt(0), op);
            Assert.True(gen.SetEquals(new HashSet<Guid> { ass1.Label }));
            Assert.True(kill.SetEquals(new HashSet<Guid> { }));

            (gen, kill) = tf.GetGenAndKill(cfg.CFGNodes.ElementAt(0), op);
            Assert.True(gen.SetEquals(new HashSet<Guid> {  ass2.Label}));
            Assert.True(kill.SetEquals(new HashSet<Guid> { }));

            (gen, kill) = tf.GetGenAndKill(cfg.CFGNodes.ElementAt(0), op);
            Assert.True(gen.SetEquals(new HashSet<Guid> { ass3.Label  }));
            Assert.True(kill.SetEquals(new HashSet<Guid> {  }));

            (gen, kill) = tf.GetGenAndKill(cfg.CFGNodes.ElementAt(0), op);
            Assert.True(gen.SetEquals(new HashSet<Guid> { ass4.Label  }));
            Assert.True(kill.SetEquals(new HashSet<Guid> {  }));

            (gen, kill) = tf.GetGenAndKill(cfg.CFGNodes.ElementAt(0), op);
            Assert.True(gen.SetEquals(new HashSet<Guid> {  ass5.Label, ass6.Label }));
            Assert.True(kill.SetEquals(new HashSet<Guid> { }));


            var outset = tf.Transfer(cfg.CFGNodes.ElementAt(0), op.Lower, op);
            var inout = (new IterativeAlgorithm()).Analyze(cfg, op, tf);

            var trueInOut = new DFA.InOutData<HashSet<System.Guid>>();
            trueInOut.Add(cfg.CFGNodes.ElementAt(0), 
                (new HashSet<Guid>(),
                 new HashSet<Guid> { ass1.Label })
            );
            trueInOut.Add(cfg.CFGNodes.ElementAt(1), 
                (new HashSet<Guid> { ass1.Label , ass2.Label, ass3.Label, ass4.Label }, 
                new HashSet<Guid> { ass1.Label, ass2.Label, ass3.Label, ass4.Label })
            );
            trueInOut.Add(cfg.CFGNodes.ElementAt(2),
                (new HashSet<Guid> { ass1.Label, ass2.Label, ass3.Label, ass4.Label },
                new HashSet<Guid> { ass1.Label, ass2.Label, ass3.Label, ass4.Label })
            );
            trueInOut.Add(cfg.CFGNodes.ElementAt(3),
                (new HashSet<Guid> { ass1.Label, ass2.Label, ass3.Label, ass4.Label },
                new HashSet<Guid> { ass1.Label, ass2.Label, ass3.Label, ass4.Label })
            );
            trueInOut.Add(cfg.CFGNodes.ElementAt(4),
                (new HashSet<Guid> { ass1.Label, ass2.Label, ass3.Label, ass4.Label },
                new HashSet<Guid> { ass1.Label, ass2.Label, ass3.Label, ass4.Label, ass5.Label, ass6.Label })
            );

            foreach (var x in cfg.CFGNodes)
                Assert.True(trueInOut[x].Item1.SetEquals(inout[x].Item1) && trueInOut[x].Item2.SetEquals(inout[x].Item2));
        }

        
        //  Переделанный пример из презентации поменял местами b2 и b3 базовые блоки
        [Test]
        public void ReachingDefenitionTest2()
        {
            var taCode = new TACode();

            var ass1 = new Assign
            {
                Left = new IntConst(1000),
                Operation = OpCode.Minus,
                Right = new IntConst(5),
                Result = new Var()
            };
            var ass2 = new Assign
            {
                Left = new IntConst(2000),
                Operation = OpCode.Copy,
                Result = new Var()
            };
            var ass3 = new Assign
            {
                Operation = OpCode.Copy,
                Right = new IntConst(3000),
                Result = new Var()
            };
            var ass4 = new Assign
            {
                Operation = OpCode.Copy,
                Right = new IntConst(4000),
                Result = ass3.Result
            };
            ass4.IsLabeled = true;
            var ass5 = new Assign
            {
                Left = ass1.Result,
                Operation = OpCode.Plus,
                Right = new IntConst(1),
                Result = ass1.Result
            };

            
            var ass6 = new Assign
            {
                Left = ass2.Result,
                Operation = OpCode.Minus,
                Right = new IntConst(4),
                Result = ass2.Result
            };
            
            var ass7 = new Assign
            {
                Operation = OpCode.Copy,
                Right = new IntConst(5000),
                Result = ass1.Result
            };
            ass7.IsLabeled = true;
            var ifgt1 = new IfGoto
            {
                Condition = new IntConst(1),
                TargetLabel = ass7.Label
            };
            var ifgt2 = new IfGoto
            {
                Condition = new IntConst(2),
                TargetLabel = ass4.Label
            };
            var empty = new Empty { };

            taCode.AddNode(ass1);
            taCode.AddNode(ass2);
            taCode.AddNode(ass3);
            taCode.AddNode(ass4);
            taCode.AddNode(ifgt1);
            taCode.AddNode(ass5);
            taCode.AddNode(ass6);
            taCode.AddNode(ass7);
            taCode.AddNode(ifgt2);
            taCode.AddNode(empty);

            var cfg = new ControlFlowGraph(taCode);
            taCode.CreateBasicBlockList();

            var op = new Operations(taCode);
            var tf = new TransferFunction();


            var (gen, kill) = tf.GetGenAndKill(cfg.CFGNodes.ElementAt(0), op);
            Assert.True(gen.SetEquals(new HashSet<Guid> { ass1.Label, ass2.Label, ass3.Label }));
            Assert.True(kill.SetEquals(new HashSet<Guid> { ass4.Label, ass5.Label, ass6.Label, ass7.Label }));

            (gen, kill) = tf.GetGenAndKill(cfg.CFGNodes.ElementAt(1), op);
            Assert.True(gen.SetEquals(new HashSet<Guid> { ass6.Label }));
            Assert.True(kill.SetEquals(new HashSet<Guid> { ass3.Label }));

            (gen, kill) = tf.GetGenAndKill(cfg.CFGNodes.ElementAt(2), op);
            Assert.True(gen.SetEquals(new HashSet<Guid> { ass4.Label, ass5.Label }));
            Assert.True(kill.SetEquals(new HashSet<Guid> { ass1.Label, ass2.Label, ass7.Label }));

            (gen, kill) = tf.GetGenAndKill(cfg.CFGNodes.ElementAt(3), op);
            Assert.True(gen.SetEquals(new HashSet<Guid> { ass7.Label }));
            Assert.True(kill.SetEquals(new HashSet<Guid> { ass1.Label, ass4.Label }));

            var inout = (new IterativeAlgorithm()).Analyze(cfg, op, tf);

            var trueInOut = new DFA.InOutData<HashSet<System.Guid>>();
            trueInOut.Add(cfg.CFGNodes.ElementAt(0),
                (new HashSet<Guid>(),
                 new HashSet<Guid> { ass1.Label, ass2.Label, ass3.Label })
            );
            trueInOut.Add(cfg.CFGNodes.ElementAt(1),
                (new HashSet<Guid> { ass1.Label, ass2.Label, ass3.Label, ass5.Label, ass6.Label, ass7.Label },
                new HashSet<Guid> { ass1.Label, ass2.Label,  ass5.Label, ass6.Label, ass7.Label })
            );
            trueInOut.Add(cfg.CFGNodes.ElementAt(2),
                (new HashSet<Guid> { ass1.Label, ass2.Label,  ass5.Label, ass6.Label, ass7.Label },
                new HashSet<Guid> { ass4.Label, ass5.Label, ass6.Label })
            );
            trueInOut.Add(cfg.CFGNodes.ElementAt(3),
                (new HashSet<Guid> { ass1.Label, ass2.Label, ass4.Label, ass5.Label, ass6.Label, ass7.Label },
                new HashSet<Guid> {  ass2.Label, ass5.Label, ass6.Label, ass7.Label })
            );
            trueInOut.Add(cfg.CFGNodes.ElementAt(4),
                (new HashSet<Guid> { ass2.Label, ass5.Label, ass6.Label, ass7.Label },
                new HashSet<Guid> { ass2.Label, ass5.Label, ass6.Label, ass7.Label })
            );

            foreach (var x in cfg.CFGNodes)
                Assert.True(trueInOut[x].Item1.SetEquals(inout[x].Item1) && trueInOut[x].Item2.SetEquals(inout[x].Item2));

        }
    }
}
