using System;
using System.Drawing;
using System.IO;
using System.Text;
using Compiler.Parser.AST;
using GraphVizWrapper;
using GraphVizWrapper.Commands;
using GraphVizWrapper.Queries;

namespace Compiler.IDE.Handlers
{
    class ASTHandler
    {
        public event EventHandler<Image> GenerationCompleted;


        public void GenerateASTImage(BlockNode root)
        {
            string graph = BuildDotGraph(root);
            File.WriteAllText(@"ast_graph.txt", graph);

            var getStartProcessQuery = new GetStartProcessQuery();
            var getProcessStartInfoQuery = new GetProcessStartInfoQuery();
            var registerLayoutPluginCommand = new RegisterLayoutPluginCommand(getProcessStartInfoQuery, getStartProcessQuery);
            var wrapper = new GraphGeneration(getStartProcessQuery, getProcessStartInfoQuery, registerLayoutPluginCommand);
            wrapper.RenderingEngine = Enums.RenderingEngine.Dot;
            byte[] output = wrapper.GenerateGraph(graph, Enums.GraphReturnType.Png);
            

            using (MemoryStream stream = new MemoryStream(output))
            {
                Image image = Image.FromStream(stream);
                GenerationCompleted(null, image);
            }
        }

        private string BuildDotGraph(BlockNode root)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("digraph AST {");
            sb.AppendLine("node[shape = record]\n");
            sb.AppendLine("graph [splines=ortho, nodesep=1, overlap=false];");
            ASTGraphvizVisitor nodeVisitor = new ASTGraphvizVisitor();
            root.Visit(nodeVisitor);
            sb.AppendLine(nodeVisitor.Nodes);
            sb.AppendLine("\n");
            sb.AppendLine(nodeVisitor.Edges);
            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}
