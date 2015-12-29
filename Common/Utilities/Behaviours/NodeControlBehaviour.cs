using Common.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Common.Utilities.Behaviours
{
    public static class NodeControlBehaviour
    {
        public static readonly DependencyProperty NodeClickedCommand = EventBehaviourFactory.CreateCommandExecutionEventBehaviour(NodeControl.MouseLeftButtonDownEvent, "NodeClickedCommand", typeof(NodeControlBehaviour));

        public static ICommand GetNodeClickedCommand(DependencyObject o)
        {
            return o.GetValue(NodeClickedCommand) as ICommand;
        }

        public static void SetNodeClickedCommand(DependencyObject o, ICommand value)
        {
            o.SetValue(NodeClickedCommand, value);
        }
    }
}
