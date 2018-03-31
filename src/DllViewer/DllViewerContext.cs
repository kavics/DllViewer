using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DllViewer
{
    public class DllViewerContext
    {
        public string[] CommandLine { get; }

        public AssemblyInfo SelectedAssembly { get; set; }
        public AssemblyInfo[] Assemblies { get; set; }

        public string Location { get; set; }

        public DllViewerContext(string[] commandLineArguments = null)
        {
            CommandLine = commandLineArguments ?? new string[0];
        }
    }
}
