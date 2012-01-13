using System;
using System.Collections.Generic;
using EnvDTE;
using EnvDTE80;

namespace IvyVisual.Commands
{
    /// <summary>
    /// SolutionExplorerSelectedItemsBase represents the base class for all VsTortoise commands
    /// that deal with selected items in the Solution Explorer.
    /// </summary>
    public abstract class SolutionExplorerSelectedItemsBase : CommandBase
    {
        /// <summary>
        /// Gets/sets if the command supports project items.
        /// </summary>
        protected bool SupportProject
        {
            get;
            set;
        }

        /// <summary>
        /// Gets/sets if the command supports solution items.
        /// </summary>
        protected bool SupportSolution
        {
            get;
            set;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SolutionExplorerSelectedItemsBase(DTE2 application, string name, string caption, string tooltip)
            : base(application, name, caption, tooltip)
        {
            // support every item kind by default.
            // derived classes can use these properties to specify in which items they are interested.
            SupportProject = true;
            SupportSolution = true;
        }

        /// <summary>
        /// Gets whether the specified item is supported by this command.
        /// </summary>
        protected bool IsSupported(EnvDTE.SelectedItem item)
        {
            switch (item.GetKind())
            {
                case SelectedItemKind.Project:
                    return SupportProject;

                case SelectedItemKind.Solution:
                    return SupportSolution;
            }

            return false;
        }

        /// <summary>
        /// Derived classes should not override this method, they should implement
        /// their query status logic in QueryContextSensitiveStatus() and QueryContextInsensitiveStatus() instead.
        /// </summary>
        public override vsCommandStatus QueryStatus()
        {
            if (Application.SelectedItems.Count > 0)
            {
                SelectedItem item = Application.SelectedItems.Item(1); // Item(index) offset is one-based, not zero-based.
                if (IsSupported(item))
                {
                    if (Application.SelectedItems.Count == 1)
                        return QueryContextSensitiveStatus(); // If it is one selected item only we perform sensitive status

                    return QueryContextInsensitiveStatus();
                }
            }

            return vsCommandStatus.vsCommandStatusUnsupported;
        }

        /// <summary>
        /// Gets the context sensitive command status. This is only performed when exactly
        /// one physical file is selected. Derived classes override this method to implement
        /// their logic. Usually this method performs various svn commands for the file in question,
        /// so we don't want this to happen for multiple files for performance reasons.
        /// </summary>
        protected virtual vsCommandStatus QueryContextSensitiveStatus()
        {
            return vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled;
        }

        /// <summary>
        /// Gets the context insensitive command status. This is when more than one item has been selected.
        /// </summary>
        protected virtual vsCommandStatus QueryContextInsensitiveStatus()
        {
            return vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled;
        }
    }
}
