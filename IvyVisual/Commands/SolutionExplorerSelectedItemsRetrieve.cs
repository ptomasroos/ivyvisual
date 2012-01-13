using System;
using System.Collections.Generic;
using System.IO;
using EnvDTE;
using EnvDTE100;
using EnvDTE80;

namespace IvyVisual.Commands
{
    public class SolutionExplorerSelectedItemsRetrieve : SolutionExplorerSelectedItemsBase
    {
        public SolutionExplorerSelectedItemsRetrieve(DTE2 application, string name, string caption, string tooltip)
            : base(application, name, caption, tooltip)
        {
        }

        public override bool Exec()
        {
            var solution = Globals.DTE.Solution as Solution4;

            Projects projects = solution.Projects;

            foreach (Project project in projects)
            {
                Output.Instance.WriteLine("Executing retrieve command for " + project.Name);
                ExecuteRetrieveCommand(project);
            }

            return true;
        }

        private static void ExecuteRetrieveCommand(Project project)
        {
            var process = new System.Diagnostics.Process();
            process.StartInfo.WorkingDirectory = Path.GetDirectoryName(project.FullName);
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/c " + Options.Instance.RetrieveCommand;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            Output.Instance.WriteLine("Output from retrieve command: " + output);
        }
    }
}
