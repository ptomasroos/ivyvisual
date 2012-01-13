using System;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.CommandBars;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace IvyVisual
{
    public class Output
    {
        private OutputWindowPane _outputPane;
        private OutputWindowPanes _outputPanes;

        private static readonly Output instance = new Output();

        public static Output Instance
        {
            get
            {
                return instance;
            }
        }

        protected Output()
        {
            OutputWindow outputWindow = Globals.DTE.Windows.Item(EnvDTE.Constants.vsWindowKindOutput).Object as OutputWindow;
            _outputPanes = outputWindow.OutputWindowPanes;

            CreateOutputPane();
        }

        private void CreateOutputPane()
        {
            _outputPane = _outputPanes.Add("IvyVisual");

            if (_outputPane != null)
            {
                _outputPane.Activate();
            }
        }

        public void Clear()
        {
            if (_outputPane != null)
                _outputPane.Clear();
        }

        public void Write(string text)
        {
            OutputString(text);
        }

        public void WriteLine(string text)
        {
            OutputString(text + Environment.NewLine);
        }

        private void OutputString(string text)
        {
            System.Diagnostics.Debug.Write(text);

            if (_outputPane != null)
                _outputPane.OutputString(text);
        }
    }
}
