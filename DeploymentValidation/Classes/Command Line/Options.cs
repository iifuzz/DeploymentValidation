using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace DeploymentValidation.Classes.Command_Line
{
    class Options
    {
        [Option(
            Required = true,
            HelpText = "Path to the script.")]
        public string Script { get; set; }
    }
}
