using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EnvDTE;

namespace IvyVisual.OptionPages
{
    public partial class General : UserControl, IDTToolsOptionsPage
    {
        public General()
        {
            InitializeComponent();
        }

        #region IDTToolsOptionsPage Members

        public void GetProperties(ref object PropertiesObject)
        {
        }

        public void OnAfterCreated(DTE DTEObject)
        {
            autoResolve.Checked = Options.Instance.ResolveAndUnresolveReferencesOnOpenAndClose;
            dependenciesRetrievePattern.Text = Options.Instance.DependenciesPattern;
            workspacePath.Text = Options.Instance.WorkspacePath;
            retrieveCommand.Text = Options.Instance.RetrieveCommand;
        }

        public void OnCancel()
        {
        }

        public void OnHelp()
        {
        }

        public void OnOK()
        {
            Options.Instance.ResolveAndUnresolveReferencesOnOpenAndClose = autoResolve.Checked;
            Options.Instance.DependenciesPattern = dependenciesRetrievePattern.Text;
            Options.Instance.WorkspacePath = workspacePath.Text;
            Options.Instance.RetrieveCommand = retrieveCommand.Text;
            Options.Instance.Save();
        }

        #endregion
    }
}
