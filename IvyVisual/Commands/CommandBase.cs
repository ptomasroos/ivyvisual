using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;
using EnvDTE80;

namespace IvyVisual.Commands
{
    public abstract class CommandBase
    {
        #region Public fields


        public string Name
        {
            get;
            private set;
        }

        public string Caption
        {
            get;
            private set;
        }

        public string Tooltip
        {
            get;
            private set;
        }

        public string PreferredBinding
        {
            get;
            protected set;
        }

        /// <summary>
        /// Specifies the command depends on all document have been saved.
        /// </summary>
        public bool SaveRequired
        {
            get;
            protected set;
        }

        #endregion

        #region Protected fields
        protected DTE2 Application { get; private set; }
        #endregion

        #region Public methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="application">Reference to the DTE2 instance</param>
        /// <param name="id">Identifier of the command. To keep it simple, you should use the class-name.</param>
        public CommandBase(DTE2 application, string name, string caption, string tooltip)
        {
            Application = application;
            Name = name;
            SaveRequired = true;
            Caption = caption;
            Tooltip = tooltip;
            PreferredBinding = string.Empty;
        }

        /// <summary>
        /// Gets status of the command.
        /// Whenever a command button is about to appear, Visual Studio queries its status.
        /// </summary>
        public abstract vsCommandStatus QueryStatus();

        /// <summary>
        /// Executes the command.
        /// Only commands with vsCommandStatus.vsCommandStatusEnabled status flag are being executed.
        /// </summary>
        /// <returns></returns>
        public abstract bool Exec();
        #endregion
    }
}
