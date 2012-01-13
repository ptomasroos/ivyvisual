using System;
using System.Collections.Generic;
using EnvDTE;
using EnvDTE80;

namespace IvyVisual.Commands
{
    public class SolutionExplorerSelectedItemsUnresolve : SolutionExplorerSelectedItemsBase
    {
        public SolutionExplorerSelectedItemsUnresolve(DTE2 application, string name, string caption, string tooltip)
            : base(application, name, caption, tooltip)
        {
        }

        public override bool Exec()
        {
            IvySolutionHandler.Unresolve(); 
            return true;
        }
    }
}
