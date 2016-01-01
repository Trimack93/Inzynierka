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
    public static class EdgeControlBehaviour
    {
        public static readonly DependencyProperty EdgeClickedCommand = EventBehaviourFactory.CreateCommandExecutionEventBehaviour(EdgeControl.MouseLeftButtonDownEvent, "EdgeClickedCommand", typeof(EdgeControlBehaviour));

        public static ICommand GetEdgeClickedCommand(DependencyObject o)
        {
            return o.GetValue(EdgeClickedCommand) as ICommand;
        }

        public static void SetEdgeClickedCommand(DependencyObject o, ICommand value)
        {
            o.SetValue(EdgeClickedCommand, value);
        }
    }
}
