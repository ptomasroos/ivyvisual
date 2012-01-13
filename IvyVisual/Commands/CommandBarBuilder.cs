using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.CommandBars;

namespace IvyVisual.Commands
{
    /// <summary>
    /// CommandBarBuilder is a helper to add controls to CommandBar's.
    /// 
    /// It keeps a list of created Controls in order to remove them when the add-in is unloaded.
    /// Adding Controls at startup and removing them when the add-in is unloaded is called the "temporary UI approach".
    /// 
    /// Further information on this topic can be found at:
    ///   HOWTO: Adding buttons, commandbars and toolbars to Visual Studio .NET from an add-i
    ///          http://www.mztools.com/articles/2005/MZ2005003.aspx
    /// </summary>
    public class CommandBarBuilder
    {
        private List<CommandBarControl> createdControls;
        private DTE2 dte { get; set; }
        private AddIn addIn { get; set; }

        public CommandBarBuilder(DTE2 dte, AddIn addin)
        {
            this.dte = dte;
            this.addIn = addin;

            createdControls = new List<CommandBarControl>();
        }

        public void RemoveCreatedControls()
        {
            for (int n = createdControls.Count - 1; n >= 0; n--)
            {
                CommandBarControl control = createdControls[n];
                control.Delete(true);
            }
            createdControls.Clear();
        }

        public CommandBarPopup CreatePopup(CommandBar commandBar, string shortName, string caption, bool beginGroup, int position)
        {
            CommandBarPopup popup = (CommandBarPopup)commandBar.Controls.Add(MsoControlType.msoControlPopup, System.Type.Missing, System.Type.Missing, position, true);

            if (popup != null)
            {
                createdControls.Add(popup);

                popup.CommandBar.Name = shortName;
                popup.Caption = caption;
                popup.BeginGroup = beginGroup;
            }

            return popup;
        }

        public CommandBarPopup CreatePopup(CommandBarPopup parent, string shortName, string caption, bool beginGroup, int position)
        {
            System.Diagnostics.Debug.Assert(parent != null);

            CommandBarPopup popup = (CommandBarPopup)parent.Controls.Add(MsoControlType.msoControlPopup,
                System.Type.Missing, System.Type.Missing, position, true);

            if (popup != null)
            {
                createdControls.Add(popup);

                popup.CommandBar.Name = shortName;
                popup.Caption = caption;
                popup.BeginGroup = beginGroup;
            }

            return popup;
        }

        public CommandBarButton CreateButton(CommandBarPopup dest, string shortName, bool beginGroup)
        {
            return CreateButton(dest, shortName, beginGroup, dest.CommandBar.Controls.Count + 1);
        }

        public CommandBarButton CreateButton(CommandBarPopup dest, string shortName, bool beginGroup, int position)
        {
            Commands2 commands = dte.Commands as Commands2;
            Command command = commands.GetCommand(commands.GetFullname(addIn, shortName));
            if (command != null)
            {
                CommandBarButton button = command.AddControl(dest.CommandBar, position) as CommandBarButton;
                if (button != null)
                {
                    createdControls.Add(button);

                    button.BeginGroup = beginGroup;
                    return button;
                }
            }

            return null;
        }

        public CommandBarButton CreateButton(CommandBar dest, string shortName, bool beginGroup, int position)
        {
            Commands2 commands = dte.Commands as Commands2;
            Command command = commands.GetCommand(commands.GetFullname(addIn, shortName));
            if (command != null)
            {
                CommandBarButton button = command.AddControl(dest, position) as CommandBarButton;
                if (button != null)
                {
                    createdControls.Add(button);

                    button.BeginGroup = beginGroup;
                    return button;
                }
            }

            return null;
        }
    }
}

