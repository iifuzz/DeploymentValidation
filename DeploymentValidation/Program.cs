using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandLine;
using DeploymentValidation.Classes.Command_Line;
using DeploymentValidation.Classes.Extensions;
using DeploymentValidation.Classes.ValidationScript;
using DeploymentValidation.Classes.ValidationScript.Messages;
using DeploymentValidation.Classes.ValidationScript.Operations;

namespace DeploymentValidation
{
    class Program
    {

        /// <summary>
        /// Returns the full path string
        /// </summary>
        /// <param name="basePath">Base path</param>
        /// <param name="relativePath">Relative path</param>
        public static string RelativeToAbsolute(string basePath, string relativePath)
        {
            var absolutePath = Path.Combine(basePath, relativePath);
            return Path.GetFullPath((new Uri(absolutePath)).LocalPath);
        }

        static void Main(string[] args)
        {
            MessageManager.Initialize();
            
            List<string> argsList = System.Environment.CommandLine.ParseArguments().ToList();
            argsList.RemoveAt(0);
            args = argsList.ToArray();

            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(options =>
                {
                    string scriptPath = RelativeToAbsolute(Path.GetDirectoryName( System.Reflection.Assembly.GetEntryAssembly().Location), options.Script);

                    Script script = Script.FromFile(scriptPath);
                    var operations = script.GetOperations();

                    foreach (IOperation operation in operations) 
                        operation.Process();


                });

            
        }
    }
}
