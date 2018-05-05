using Compiler.Parser.AST;
using Compiler.Parser.Visitors;
using System;

namespace Compiler.IDE.Handlers
{
    class ThreeAddrCodeHandler
    {
        public event EventHandler<ThreeAddrCode.TACode> GenerationCompleted;
        public event EventHandler<string> PrintableCodeGenerated;

        public void GenerateThreeAddrCode(object sender, BlockNode e)
        {
            var visitor = new TACodeVisitor();
            e.Visit(visitor);

            GenerationCompleted(null, visitor.Code);

            PostProcess(visitor.Code);
        }

        private void PostProcess(ThreeAddrCode.TACode code)
        {
            String postProcessCode = code.ToString().Replace($"\"{Environment.NewLine}\"", "\"%NEW_LINE%\"");
            PrintableCodeGenerated(null, postProcessCode);
        }
    }
}
