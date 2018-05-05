using System;
using System.Drawing;
using System.IO;
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

            var getStartProcessQuery = new GetStartProcessQuery();
            var getProcessStartInfoQuery = new GetProcessStartInfoQuery();
            var registerLayoutPluginCommand = new RegisterLayoutPluginCommand(getProcessStartInfoQuery, getStartProcessQuery);

            // GraphGeneration can be injected via the IGraphGeneration interface

            var wrapper = new GraphGeneration(getStartProcessQuery, getProcessStartInfoQuery, registerLayoutPluginCommand);
            wrapper.RenderingEngine = Enums.RenderingEngine.Dot;
            byte[] output = wrapper.GenerateGraph("digraph{a -> b; b -> c; c -> a;}", Enums.GraphReturnType.Png);
            

            using (MemoryStream stream = new MemoryStream(output))
            {
                Image image = Image.FromStream(stream);
                GenerationCompleted(null, image);
            }

        }
    }
}
