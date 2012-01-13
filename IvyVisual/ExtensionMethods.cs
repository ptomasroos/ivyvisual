using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.CommandBars;
using IvyVisual.Commands;

namespace IvyVisual
{
    public static class CommandBarControlExtensionMethods
    {
        public static Command GetCommand(this CommandBarControl me)
        {
            Command command = null;

            try
            {
                int id;
                string guid;
                DTE2 dte = me.Application as DTE2;

                dte.Commands.CommandInfo(me, out guid, out id);
                command = dte.Commands.Item(guid, id);
            }
            catch
            { }

            return command;
        }
    }

    public static class CommandBarExtensionMethods
    {
        public static CommandBarButton GetButton(this CommandBar me, string commandName)
        {
            // http://social.msdn.microsoft.com/Forums/en-US/vsx/thread/65bb4595-240e-4147-94d4-4fd762476eb2
            foreach (CommandBarControl control in me.Controls)
            {
                if (control is CommandBarButton)
                {
                    Command command = control.GetCommand();
                    if (command != null)
                    {
                        if (string.Compare(command.Name, commandName) == 0)
                            return control as CommandBarButton;
                    }
                }
            }

            return null;
        }
    }

    public static class Commands2ExtensionMethods
    {
        /// <summary>
        /// Gets the Command specified by shortName..
        /// </summary>
        /// <param name="shortName">Command short name</param>
        /// <returns>Returns the Command on success or null when the Command does not exist.</returns>
        public static Command GetCommand(this Commands2 me, string fullname)
        {
            Command command = null;

            // Try to retrieve the command, ignore any exception that would
            // occur if the command was not created yet.
            try
            {
                command = me.Item(fullname, -1);
            }
            catch
            { }

            return command;
        }

        public static Command GetCommandByBinding(this Commands2 me, string binding)
        {
            if (!string.IsNullOrEmpty(binding))
            {
                foreach (Command command in me)
                {
                    object[] bindings = (object[])command.Bindings;

                    foreach (object b in bindings)
                    {
                        if (string.Compare(binding, b as string, true) == 0)
                            return command;
                    }
                }
            }

            return null;
        }

        public static string GetFullname(this Commands2 me, AddIn addin, string shortName)
        {
            return string.Format("{0}.{1}", addin.ProgID, shortName);
        }

        public static Command CreateCommandButton(this Commands2 me, AddIn addin, CommandBase command)
        {
            object[] contextGUIDS = new object[] { };
            Commands2 commands = me;

            // Try to retrieve the command. If it was created already, just leave since the command exists.
            Command vsCommand = me.GetCommand(me.GetFullname(addin, command.Name));
            if (vsCommand != null)
                return null; // it already exists

            try
            {
                // Try to create a command with icon. 
                // This usually fails using Visual Studio 2005 if the VsTortoise satellite dll culture does not match the one of Visual Studio.
                vsCommand = commands.AddNamedCommand2(addin, command.Name, command.Caption, command.Tooltip, false, Type.Missing,
                    ref contextGUIDS, (int)vsCommandStatus.vsCommandStatusSupported + (int)vsCommandStatus.vsCommandStatusEnabled,
                    (int)vsCommandStyle.vsCommandStylePictAndText, vsCommandControlType.vsCommandControlTypeButton);
            }
            catch
            { }

            if (vsCommand == null)
            {
                // We're here when command creation failed, so create it without icon.
                vsCommand = commands.AddNamedCommand2(addin, command.Name, command.Caption, command.Tooltip, true, (int)0, 
                    ref contextGUIDS, (int)vsCommandStatus.vsCommandStatusSupported + (int)vsCommandStatus.vsCommandStatusEnabled, 
                    (int)vsCommandStyle.vsCommandStylePictAndText, vsCommandControlType.vsCommandControlTypeButton);
            }

            if (vsCommand != null)
            {
                // First check if there is a preferred binding at all,
                // to skip unnecessary further expensive tests
                if (!string.IsNullOrEmpty(command.PreferredBinding))
                {
                    // The command inside VS has been added.
                    // Now check if any command is using the preferred binding already.
                    // We use it only, when it is not used by another command!
                    // Bindings are "atomic". If we would use a binding that is in use already,
                    // the binding gets removed from the current command. We don't want to steal bindings from other commands!
                    bool bindingUsedAlready = me.GetCommandByBinding(command.PreferredBinding) != null;
                    if (!bindingUsedAlready)
                    {
                        try
                        {
                            // TODO: figure how to assign bindings correctly
                            // Bindings use translated texts, this is so ridiculous.
                            // I don't know how to assign a shortcut in a language independent way,
                            // so we use a try/catch here, otherwise the add-in refuses to start-up.
                            // English: "Text Editor::Ctrl+Shift+Alt+Up Arrow"
                            // German.: "Text-Editor::Strg+Umschalt+Alt+NACH-OBEN-TASTE"
                            vsCommand.Bindings = command.PreferredBinding;
                        }
                        catch
                        { }
                    }
                }
            }

            return vsCommand;
        }
    }

    public static class DTE2ExtensionMethods
    {
        public static bool HasSolution(this DTE2 dte)
        {
            if (dte.Solution == null)
                return false;

            return dte.Solution.IsOpen && !string.IsNullOrEmpty(dte.Solution.FullName);
        }

        /// <summary>
        /// Gets whether a project file is active that exists on the file system.
        /// </summary>
        public static bool HasProject(this DTE2 dte)
        {
            if (!dte.HasSolution())
                return false;

            return dte.ActiveSolutionProjects != null &&
                dte.Solution.Projects != null && dte.Solution.Projects.Count > 0;
        }

        public static Project ActiveProject(this DTE2 dte)
        {
            Array projects = dte.ActiveSolutionProjects as Array;
            if (projects.Length > 0)
                return projects.GetValue(0) as Project;

            return null;
        }
    }

    public enum SelectedItemKind
    {
        Project,
        Solution,
        Unknown,
    }

    public static class SelectedItemExtensionMethods
    {
        public static bool IsProject(this EnvDTE.SelectedItem me)
        {
            if (me.Project != null && me.ProjectItem == null)
                return true;

            return false;
        }

        public static bool IsSolution(this EnvDTE.SelectedItem me)
        {
            if (me.ProjectItem == null && me.Project == null)
                return true;

            return false;
        }

        public static SelectedItemKind GetKind(this EnvDTE.SelectedItem me)
        {
            if (me.IsProject())
                return SelectedItemKind.Project;

            if (me.IsSolution())
                return SelectedItemKind.Solution;

            return SelectedItemKind.Unknown;
        }

        //public static int GetFullNames(this EnvDTE.SelectedItem me, ICollection<string> dest)
        //{
        //    int beforeCount = dest.Count;

        //    switch (me.GetKind())
        //    {
        //        case SelectedItemKind.Project:
        //            me.Project.GetDirectoryNames(dest);
        //            break;

        //        case SelectedItemKind.Solution:
        //            me.DTE.Application.Solution.GetDirectoryNames(dest);
        //            break;
        //    }

        //    int addedCount = dest.Count - beforeCount;
        //    return addedCount;
        //}
    }
}
