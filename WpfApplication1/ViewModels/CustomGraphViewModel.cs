using Common.Models;
using Common.Utilities;
using MvvmDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApplication1.Models.Algorithms;

namespace WpfApplication1.ViewModels
{
    public class CustomGraphViewModel : LearningViewModel
    {
        public CustomGraphViewModel(string algorithmName, Graph graph)
        {
            //this.AreInstructionsVisible = false;
            this.AlgorithmName = algorithmName;
            
            try
            {
                Func<INotifyPropertyChanged, Type> typeLocator = (t) => App.GetViewClassTypeLocalizer(t);
                _dialogService = new DialogService(null, typeLocator);

                List<Graph> graphList = new List<Graph>();
                graphList.Add(graph);
                InitializeAlgorithmFromGraph(graphList);

                this.Instruction = _algorithm?.GetCurrentInstruction();

                RaisePropertyChanged("CanvasNodes");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wystąpil błąd: " + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);

                // Get the learning view window from static property and close it
                var openedWindows = App.Current.Windows;
                Window currentWindow = null;

                for (int i = 0; i < openedWindows.Count; i++)
                {
                    if (openedWindows[i].DataContext?.GetType().Name == this.GetType().Name)
                    {
                        currentWindow = openedWindows[i];
                        break;
                    }
                }
                // Can't close a window which practically wasn't opened yet. 
                currentWindow.Loaded += new RoutedEventHandler((sender, e) => { (sender as Window).Close(); });         // So let's add closing handler to its Loaded event. (ProjektBD experience~ )
            }
        }
    }
}
