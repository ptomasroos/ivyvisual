using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;
using EnvDTE80;

namespace IvyVisual.Commands
{
    public class CommandManager
    {
        private Dictionary<string, CommandBase> commands;
        private DTE2 dte { get; set; }
        private AddIn addIn { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public CommandManager(DTE2 dte, AddIn addin)
        {
            this.dte = dte;
            this.addIn = addin;

            commands = new Dictionary<string, CommandBase>();
        }

        /// <summary>
        /// Adds a command to the manager.
        /// </summary>
        /// <param name="command">Reference to the command to add</param>
        public void Add(CommandBase command)
        {
            Commands2 vsCommands = (Commands2)dte.Commands;

            // Get full name how Visual Studio queries this command
            string fullname = vsCommands.GetFullname(addIn, command.Name);

            // Add to our own VsTortoise command management
            commands[fullname] = command;

            // Create the command inside Visual Studio permanently
            Command vsCommand = vsCommands.CreateCommandButton(addIn, command);
        }

        /// <summary>
        /// Gets status of the command with the specified full name.
        /// Whenever a command button is about to appear, Visual Studio
        /// queries its status.
        /// </summary>
        public vsCommandStatus QueryStatus(string fullname)
        {
            if (IsPresent(fullname))
                return commands[fullname].QueryStatus();

            return vsCommandStatus.vsCommandStatusUnsupported;
        }

        /// <summary>
        /// Executes the command with the specified full name.
        /// </summary>
        public bool Exec(string fullname)
        {
            if (IsPresent(fullname))
            {
                CommandBase command = commands[fullname];

                return command.Exec();
            }

            return false;
        }

        /// <summary>
        /// Checks whether the command with the specified full name exists.
        /// </summary>
        public bool IsPresent(string fullname)
        {
            return commands.ContainsKey(fullname);
        }

        private string GetQualifiedName(string shortName)
        {
            return addIn.ProgID + shortName;
        }
    }
}
