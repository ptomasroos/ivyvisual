using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using EnvDTE100;
using EnvDTE80;

namespace IvyVisual
{
    public class Options : ISerializable
    {
        private static readonly Options instance = new Options();
        private static FileInfo loadedSettingsFile;

        public static Options Instance
        {
            get
            {
                return instance;
            }
        }

        public bool ResolveAndUnresolveReferencesOnOpenAndClose { get; set; }
        public string DependenciesPattern { get; set; }
        public string WorkspacePath { get; set; }
        public string RetrieveCommand { get; set; }

        public void Load()
        {
            DTE2 dte = Globals.DTE;

            if (!dte.HasSolution())
                return;

            Solution4 solution = dte.Solution as Solution4;

            var solutionDirectory = new DirectoryInfo(Path.GetDirectoryName(solution.FullName));

            var parentSettingsFile = new FileInfo(
                solutionDirectory.Parent.FullName + 
                Path.DirectorySeparatorChar + 
                "default.ivyvisual");

            FileInfo settingsFileToDeserialize = null;

            if (parentSettingsFile.Exists)
            {
                settingsFileToDeserialize = parentSettingsFile;
            }
            else
            {
                var settingsFile = new FileInfo(solution.FullName + ".ivyvisual");

                if (settingsFile.Exists)
                {
                    settingsFileToDeserialize = settingsFile;
                }
                else
                {
                    return;
                }
            }

            loadedSettingsFile = settingsFileToDeserialize;
            Options options = XmlFileSerializer<Options>.Deserialize(settingsFileToDeserialize.FullName);

            Instance.ResolveAndUnresolveReferencesOnOpenAndClose = options.ResolveAndUnresolveReferencesOnOpenAndClose;
            Instance.DependenciesPattern = options.DependenciesPattern;
            Instance.WorkspacePath = options.WorkspacePath;
            Instance.RetrieveCommand = options.RetrieveCommand;
        }

        public void Save()
        {
            DTE2 dte = Globals.DTE;

            if (!dte.HasSolution())
                return;

            Solution4 solution = dte.Solution as Solution4;

            XmlFileSerializer<Options>.Serialize(loadedSettingsFile.FullName, Options.Instance);
        }

        #region ISerializable Members

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.SetType(typeof(OptionsSerializationHelper));
        }

        #endregion
    }

    public class OptionsSerializationHelper : IObjectReference
    {
        #region IObjectReference Members

        public object GetRealObject(StreamingContext context)
        {
            return Options.Instance;
        }

        #endregion
    }
}
