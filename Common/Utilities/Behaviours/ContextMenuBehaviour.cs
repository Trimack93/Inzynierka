using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Common.Utilities.Behaviours
{
    public static class ContextMenuBehaviour
    {
        public static readonly DependencyProperty ContextMenuOpenedCommand = EventBehaviourFactory.CreateCommandExecutionEventBehaviour(ContextMenu.OpenedEvent, "ContextMenuOpenedCommand", typeof(ContextMenuBehaviour));

        public static ICommand GetContextMenuOpenedCommand(DependencyObject o)
        {
            return o.GetValue(ContextMenuOpenedCommand) as ICommand;
        }

        public static void SetContextMenuOpenedCommand(DependencyObject o, ICommand value)
        {
            o.SetValue(ContextMenuOpenedCommand, value);
        }
    }
}
