using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE100;
using EnvDTE;
using IvyVisual.IvyModel;
using VSLangProj80;
using VSLangProj;
using System.IO;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace IvyVisual
{
    public class IvySolutionHandler
    {
        private static List<KeyValuePair<IvyModule, string>> GetIvyModules(Project project)
        {
            var ivyModules = new List<KeyValuePair<IvyModule, string>>();

            var projectDirectory = new DirectoryInfo(Path.GetDirectoryName(project.FullName));
            var workspaceDirectory = new DirectoryInfo(
                (!string.IsNullOrEmpty(Options.Instance.WorkspacePath) 
                ? Options.Instance.WorkspacePath 
                : projectDirectory.Parent.FullName));

            if (!workspaceDirectory.Exists)
            {
                Output.Instance.WriteLine("The specified workspace directory doesn't exist, can't continue looking for ivy.xml files");
                return ivyModules;
            }

            FileInfo[] ivyFiles = workspaceDirectory.GetFiles("ivy.xml", SearchOption.AllDirectories);

            foreach (FileInfo ivyFile in ivyFiles)
            {
                IvyModule ivyModule = XmlFileSerializer<IvyModule>.Deserialize(ivyFile.FullName);
                ivyModules.Add(new KeyValuePair<IvyModule, string>(ivyModule, ivyFile.FullName));
            }

            return ivyModules;
        }

        public static void Resolve()
        {
            Output.Instance.WriteLine("Starting resolve of references");

            var solution = Globals.DTE.Solution as Solution4;

            Projects projects = solution.Projects;

            foreach (Project project in projects)
            {
                Output.Instance.WriteLine("Resolving project " + project.Name);

                ResolveProject(solution, projects, project);
            }
        }

        private static void ResolveProject(Solution4 solution, Projects projects, Project project)
        {
            var vsProject = project.Object as VSProject2;

            if (vsProject == null)
                return;

            // load ivymodules
            List<KeyValuePair<IvyModule, string>> ivyModules = GetIvyModules(project);

            References references = vsProject.References;

            foreach (Reference3 reference in references)
            {
                if (reference.Type == prjReferenceType.prjReferenceTypeAssembly && reference.SourceProject == null)
                {
                    KeyValuePair<IvyModule, string> matching = ivyModules.SingleOrDefault(kvp => kvp.Key.Publications.ArtifactList.Count(artifact => artifact.Filename == Path.GetFileName(reference.Path)) > 0);

                    if (matching.Key == null || matching.Value == null)
                    {
                        Output.Instance.WriteLine("Could not find a matching ivy module for reference '" + reference.Name + "'");
                        continue;
                    }

                    Output.Instance.WriteLine("Found matching ivy module for reference '" + reference.Name + "'");

                    // project base directory
                    var projectDirectory = new DirectoryInfo(Path.GetDirectoryName(matching.Value));

                    Output.Instance.WriteLine("Ivy modules base directory is '" + projectDirectory.FullName + "'");

                    if (!projectDirectory.Exists)
                    {
                        Output.Instance.WriteLine("The directory does not seem to exist, will skip this reference!");
                        continue;
                    }

                    // find the project file which exists in the directory
                    FileInfo[] projectFiles = projectDirectory.GetFiles("*.csproj", SearchOption.TopDirectoryOnly);

                    if (projectFiles.Length == 0 || projectFiles.Length > 1)
                    {
                        Output.Instance.WriteLine("Could not find a project file in the directory '" + projectDirectory.FullName + "', will skip this reference");
                        continue;
                    }

                    // get the first project file, as a rule it should only exist one
                    FileInfo projectFile = projectFiles.First();

                    Output.Instance.WriteLine("The selected project file for reference '" + reference.Name + "' is '" + projectFile.FullName + "'");

                    Project result = null;

                    // parse thru all existing projects to see if it already has been added
                    foreach (Project proj in projects)
                    {
                        if (proj.FullName == projectFile.FullName)
                            result = proj;
                    }

                    // if the project wasn't added earlier add it, and resolve its references
                    if (result == null)
                    {
                        result = solution.AddFromFile(projectFile.FullName, false);
                        ResolveProject(solution, projects, result);
                    }

                    // remove the old binary reference
                    reference.Remove();
                    vsProject.References.AddProject(result);
                }
            }
        }

        public static void Unresolve()
        {
            Output.Instance.WriteLine("Starting unresolve of project references");

            var solution = Globals.DTE.Solution as Solution4;

            Projects projects = solution.Projects;

            foreach (Project project in projects)
            {
                if (project.Name == "Miscellaneous Files")
                    continue;

                Output.Instance.WriteLine("Unresolving project " + project.Name);
                UnresolveProject(solution, projects, project);
            }

            // this is needed so that the user isnt prompted for saving the solution when unresolving
            solution.SaveAs(solution.FullName);
        }

        public static void UnresolveProject(Solution4 solution, Projects projects, Project project)
        {
            var solutionDirectoryPath = Path.GetDirectoryName(solution.FullName); // can be moved out side this method
            var projectDirectoryPath = Path.GetDirectoryName(project.FullName);

            VSProject2 vsProject = project.Object as VSProject2;

            // load ivymodules
            List<KeyValuePair<IvyModule, string>> ivyModules = GetIvyModules(project);

            References references = vsProject.References;

            // http://social.msdn.microsoft.com/Forums/en-US/vsx/thread/03d9d23f-e633-4a27-9b77-9029735cfa8d/
            string fullPath = project.Properties.Item("FullPath").Value.ToString();
            string outputPath = project.ConfigurationManager.ActiveConfiguration.Properties.Item("OutputPath").Value.ToString();
            string outputDir = Path.Combine(fullPath, outputPath);

            // very important! we need to delete the output dir since it won't otherwise add references with and hintpath, instead it references the binary to their bin\Debug directory, took forever to understand!
            Directory.Delete(outputDir, true);

            foreach (Reference3 reference in references)
            {
                // do only check for assemlby references which has is a project reference and it its directory should not be a part of the solution. since we don't want to resolve our main project
                if (reference.Type == prjReferenceType.prjReferenceTypeAssembly && reference.SourceProject != null && !reference.SourceProject.FullName.Contains(solutionDirectoryPath))
                {
                    var referenceFileName = Path.GetFileName(reference.Path);

                    IvyModule foundIvyModule = null;
                    string foundIvyModulePath = null;
                    Artifact foundIvyModuleArtifact = null;

                    foreach (KeyValuePair<IvyModule,string> kvp in ivyModules)
                    {
                        IvyModule ivyModule = kvp.Key;
                        string ivyModulePath = kvp.Value;

                        foreach (Artifact artifact in ivyModule.Publications.ArtifactList)
                        {
                            if (artifact.Filename == referenceFileName)
                            {
                                foundIvyModule = ivyModule;
                                foundIvyModulePath = ivyModulePath;
                                foundIvyModuleArtifact = artifact;
                                break;
                            }
                        }

                        if (foundIvyModule != null)
                        {
                            break;
                        }
                    }

                    if (foundIvyModule == null)
                    {
                        Output.Instance.WriteLine("Could not find a matching ivy module for reference '" + reference.Name + "'");
                        continue;
                    }

                    // the calulated path to the binary reference
                    var referenceFile = new FileInfo(GetCalculatedReferencePath(projectDirectoryPath, foundIvyModule, foundIvyModuleArtifact, referenceFileName));

                    Output.Instance.WriteLine("Found a matching ivy module for reference '" + reference.Name + "', assumed path to the binary file is '" + referenceFile.FullName + "'");

                    if (!referenceFile.Exists)
                    {
                        Output.Instance.WriteLine("The reference file does not exist, assure that you'r following the patterns for dependencies.");
                        continue;
                    }

                    reference.Remove();
                    vsProject.References.Add(referenceFile.FullName);
                }
            }

            project.Save(project.FullName);

            if (!projectDirectoryPath.Contains(solutionDirectoryPath))
            {
                solution.Remove(project);
            }
        }

        private static string GetCalculatedReferencePath(string projectDirectoryPath, IvyModule ivyModule, Artifact artifact, string referenceFileName)
        {
            string dependenciesRetrievePattern = @"Dependencies\[organisation]\[module]\[artifact].[type]";

            if (string.IsNullOrEmpty(Options.Instance.DependenciesPattern))
            {
                Output.Instance.WriteLine(@"There is no dependencies retrieve pattern set, will default to Dependencies\[organisation]\[module]\[artifact].[type]");
            }
            else
            {
                dependenciesRetrievePattern = Options.Instance.DependenciesPattern;
            }

            return projectDirectoryPath + 
                Path.DirectorySeparatorChar + 
                dependenciesRetrievePattern
                .Replace("[organisation]", ivyModule.Info.Organisation)
                .Replace("[module]", ivyModule.Info.Module)
                .Replace("[branch]", ivyModule.Info.Branch)
                .Replace("[revision]", ivyModule.Info.Revision)
                .Replace("[artifact]", artifact.Name)
                .Replace("[type]", artifact.Type)
                .Replace("[ext]", artifact.Extension)
                .Replace("[conf]", artifact.Configuration)
                .Replace("[originalname]", artifact.Filename);
        }
    }
}
