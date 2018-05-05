using Compiler.Parser.AST;
using Compiler.Parser.Visitors;
using System;

namespace Compiler.IDE.Handlers
{
    class ThreeAddrCodeHandler
    {
        public event EventHandler<String> GenerationCompleted;

        public void GenerateThreeAddrCode(object sender, BlockNode e)
        {
            var visitor = new TACodeVisitor();
            e.Visit(visitor);

            String code = visitor.Code.ToString();
            String postProcessCode = code.Replace($"\"{Environment.NewLine}\"", "\"%NEW_LINE%\"");

            GenerationCompleted(null, postProcessCode);
        }
    }
}
