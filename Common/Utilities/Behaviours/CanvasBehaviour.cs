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
    public static class CanvasBehaviour
    {
        public static readonly DependencyProperty CanvasMouseDownCommand = EventBehaviourFactory.CreateCommandExecutionEventBehaviour( Canvas.MouseDownEvent, "CanvasMouseDownCommand", typeof(CanvasBehaviour) );
        public static readonly DependencyProperty CanvasMouseMoveCommand = EventBehaviourFactory.CreateCommandExecutionEventBehaviour( Canvas.MouseMoveEvent, "CanvasMouseMoveCommand", typeof(CanvasBehaviour) );
        public static readonly DependencyProperty CanvasMouseUpCommand = EventBehaviourFactory.CreateCommandExecutionEventBehaviour( Canvas.MouseUpEvent, "CanvasMouseUpCommand", typeof(CanvasBehaviour) );

        public static ICommand GetCanvasMouseDownCommand(DependencyObject o)
        {
            return o.GetValue(CanvasMouseDownCommand) as ICommand;
        }
        public static ICommand GetCanvasMouseMoveCommand(DependencyObject o)
        {
            return o.GetValue(CanvasMouseMoveCommand) as ICommand;
        }
        public static ICommand GetCanvasMouseUpCommand(DependencyObject o)
        {
            return o.GetValue(CanvasMouseUpCommand) as ICommand;
        }

        public static void SetCanvasMouseDownCommand(DependencyObject o, ICommand value)
        {
            o.SetValue(CanvasMouseDownCommand, value);
        }
        public static void SetCanvasMouseMoveCommand(DependencyObject o, ICommand value)
        {
            o.SetValue(CanvasMouseMoveCommand, value);
        }
        public static void SetCanvasMouseUpCommand(DependencyObject o, ICommand value)
        {
            o.SetValue(CanvasMouseUpCommand, value);
        }
    }
}
