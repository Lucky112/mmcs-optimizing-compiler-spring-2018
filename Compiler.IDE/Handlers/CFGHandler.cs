using System;
using System.Drawing;
using System.IO;
using System.Text;
using Compiler.ThreeAddrCode;
using Compiler.ThreeAddrCode.CFG;
using GraphVizWrapper;
using GraphVizWrapper.Commands;
using GraphVizWrapper.Queries;

namespace Compiler.IDE.Handlers
{
    class CFGHandler
    {
        public event EventHandler<Image> GenerationCompleted;


        public void GenerateCFGImage(TACode code)
        {
            var cfg = new ControlFlowGraph(code);

            string graph = BuildDotGraph(cfg);
            File.WriteAllText(@"cfg_graph.txt", graph);

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

        private string GetNameForGuid(Guid guid)
        {
            return "b" + guid.ToString().Substring(0, 3);
        }

        private string BuildDotGraph(ControlFlowGraph cfg)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("digraph CFG { \n");
            sb.Append("node[shape = record]\n\n");
            sb.Append("ordering=out\n");
            foreach (var node in cfg.CFGNodes)
            {
                string name = GetNameForGuid(node.BlockId);
                
                // [label = "<TEXT>\l<TEXT>"]
                sb.Append($"{name}  [label = \"");
                foreach (var cmd in node.CodeList)
                {
                    string cmdText = OutputSanitizer.Sanitize(cmd.ToString(), OutputSanitizer.SanitizeLevel.DotFile);
                    sb.Append(cmdText + @"\l");
                }
                sb.Append("\"]\n");
            }
            sb.Append("\n\n");

            // генерируем дуги
            foreach (var node in cfg.CFGNodes)
            {
                string name = GetNameForGuid(node.BlockId);

                foreach (var p in node.Parents)
                {
                    string pName = GetNameForGuid(p.BlockId);
                    sb.Append($"{name} -> {pName}[dir=both, arrowhead=none];\n");
                }

                //foreach (var c in node.Parents)
                //{
                //    string cName = GetNameForGuid(c.BlockId);
                //    sb.Append($"{name} -> {cName};\n");
                //}
            }

            sb.Append("}");

            return sb.ToString();
        }
    }
}
