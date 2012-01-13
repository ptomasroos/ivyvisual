using System;
using System.Collections.Generic;
using EnvDTE;
using EnvDTE80;

namespace IvyVisual.Commands
{
    public class SolutionExplorerSelectedItemsResolve : SolutionExplorerSelectedItemsBase
    {
        public SolutionExplorerSelectedItemsResolve(DTE2 application, string name, string caption, string tooltip)
            : base(application, name, caption, tooltip)
        {
        }

        public override bool Exec()
        {
            IvySolutionHandler.Resolve();
            return true;
        }
    }
}
