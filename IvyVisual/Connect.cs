using System;
using Extensibility;
using EnvDTE;
using EnvDTE80;
using IvyVisual.Commands;
using Microsoft.VisualStudio.CommandBars;

namespace IvyVisual
{
    /// <summary>The object for implementing an Add-in.</summary>
    /// <seealso class='IDTExtensibility2' />
    public class Connect : IDTExtensibility2, IDTCommandTarget
    {
        private SolutionEvents solutionEvents;
        private CommandManager commandManager;
        private CommandBarBuilder commandBarBuilder;

        /// <summary>Implements the constructor for the Add-in object. Place your initialization code within this method.</summary>
        public Connect()
        {
        }

        #region Private, Initialization
        private void Initialize()
        {
            CreateCommands();
            AddTemporaryUI();
        }

        private void Uninitialize()
        {
            // Remove temporary UI again
            commandBarBuilder.RemoveCreatedControls();
        }
        #endregion

        private void CreateCommands()
        {
            commandManager.Add(new SolutionExplorerSelectedItemsResolve(Globals.DTE, "SolutionExplorerSelectedItemsResolve", "Map binary references to project references", ""));
            commandManager.Add(new SolutionExplorerSelectedItemsUnresolve(Globals.DTE, "SolutionExplorerSelectedItemsUnresolve", "Map project references to binary references", ""));
            commandManager.Add(new SolutionExplorerSelectedItemsRetrieve(Globals.DTE, "SolutionExplorerSelectedItemsRetrieve", "Run retrieve command", ""));
        }

        private void AddTemporaryUI()
        {
            AddTemporarySolutionExplorerSelectedItemsUI("Solution", "IvyVisualSolutionPopup");
        }

        private void AddTemporarySolutionExplorerSelectedItemsUI(string commandBarName, string createdPopupName)
        {
            CommandBar commandBar = (Globals.DTE.CommandBars as CommandBars)[commandBarName];

            // The default position is at the context menu bottom
            int position = commandBar.Controls.Count + 1;

            CommandBarPopup ivyVisualBar = commandBarBuilder.CreatePopup(commandBar, createdPopupName, "IvyVisual", true, position);

            if (ivyVisualBar != null)
            {
                commandBarBuilder.CreateButton(ivyVisualBar, "SolutionExplorerSelectedItemsResolve", false);
                commandBarBuilder.CreateButton(ivyVisualBar, "SolutionExplorerSelectedItemsUnresolve", false);
                commandBarBuilder.CreateButton(ivyVisualBar, "SolutionExplorerSelectedItemsRetrieve", true);
            }
        }

        private void SolutionEvents_Opened()
        {
            Output.Instance.WriteLine("Loading options");
            Options.Instance.Load();

            if (Options.Instance.ResolveAndUnresolveReferencesOnOpenAndClose)
            {
                Output.Instance.WriteLine("Resolve on open and close is enabled, will trigger resolve");
                IvySolutionHandler.Resolve();
            }
        }

        private void SolutionEvents_BeforeClosing()
        {
            Output.Instance.WriteLine("Saving options");
            Options.Instance.Save();

            if (Options.Instance.ResolveAndUnresolveReferencesOnOpenAndClose)
            {
                Output.Instance.WriteLine("Resolve on open and close is enabled, will trigger unresolve");
                IvySolutionHandler.Unresolve();
            }
        }

        #region IDTExtensibility2 Members

        /// <summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.</summary>
        /// <param term='application'>Root object of the host application.</param>
        /// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
        /// <param term='addInInst'>Object representing this Add-in.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
        {
            Globals.DTE = (DTE2)application;
            Globals.Addin = (AddIn)addInInst;

            solutionEvents = Globals.DTE.Events.SolutionEvents;
            solutionEvents.Opened += new _dispSolutionEvents_OpenedEventHandler(SolutionEvents_Opened);
            solutionEvents.BeforeClosing += new _dispSolutionEvents_BeforeClosingEventHandler(SolutionEvents_BeforeClosing);

            commandManager = new CommandManager(Globals.DTE, Globals.Addin);
            commandBarBuilder = new CommandBarBuilder(Globals.DTE, Globals.Addin);

            switch (connectMode)
            {
                case ext_ConnectMode.ext_cm_UISetup:
                    // Initialize the UI of the add-in
                    break;

                case ext_ConnectMode.ext_cm_Startup:
                    // The add-in was marked to load on startup
                    // Do nothing at this point because the IDE may not be fully initialized
                    // Visual Studio will call OnStartupComplete when fully initialized
                    break;

                case ext_ConnectMode.ext_cm_AfterStartup:
                    // The add-in was loaded by hand after startup using the Add-In Manager
                    // Initialize it in the same way that when is loaded on startup
                    Initialize();
                    break;
            }
        }

        /// <summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being unloaded.</summary>
        /// <param term='disconnectMode'>Describes how the Add-in is being unloaded.</param>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
        {
        }

        /// <summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification when the collection of Add-ins has changed.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />		
        public void OnAddInsUpdate(ref Array custom)
        {
        }

        /// <summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnStartupComplete(ref Array custom)
        {
            Initialize();
        }

        /// <summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnBeginShutdown(ref Array custom)
        {
        }

        #endregion

        #region IDTCommandTarget Members

        public void Exec(string commandName, vsCommandExecOption executeOption, ref object varIn, ref object varOut, ref bool handled)
        {
            handled = commandManager.Exec(commandName);
        }

        public void QueryStatus(string commandName, vsCommandStatusTextWanted neededText, ref vsCommandStatus status, ref object commandText)
        {
            status = commandManager.QueryStatus(commandName);
        }

        #endregion
    }
}