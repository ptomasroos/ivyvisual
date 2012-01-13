using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;
using EnvDTE80;

namespace IvyVisual.Commands
{
    /// <summary>
    /// Base class for VsTortoise commands that implement functionality
    /// which relates to all items in a Visual Studio Solution.
    /// </summary>
    public abstract class SolutionBase : CommandBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SolutionBase(DTE2 application, string name, string caption, string tooltip)
            : base(application, name, caption, tooltip)
        {
        }

        /// <summary>
        /// Gets directories of projects in solution as well as the solution directory.
        /// </summary>
        //protected List<string> GetDirectoryNames(bool removeSubDirs)
        //{
        //    List<string> pathes = new List<string>(128);

        //    // Gets pathes of solution and projects
        //    //Application.Solution.GetDirectoryNames(pathes);

        //    // Remove any sub directories, because svn operations are recursive anyway.
        //    //if(removeSubDirs)
        //    //    PathHelper.RemoveSubDirectories(pathes);

        //    return pathes;
        //}

        /// <summary>
        /// Gets whether the command is supported.
        /// Solution commands are supported whenever a solution is open, no source control checks are done.
        /// </summary>
        public override vsCommandStatus QueryStatus()
        {
            if (Application.HasSolution())
                return vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled;

            return vsCommandStatus.vsCommandStatusSupported;
        }
    }
}
