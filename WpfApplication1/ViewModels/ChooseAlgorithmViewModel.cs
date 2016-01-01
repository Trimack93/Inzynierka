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
using System.Windows.Input;
using WpfApplication1.Models;

namespace WpfApplication1.ViewModels
{
    public class ChooseAlgorithmViewModel : BaseNotifyPropertyChanged
    {
        public ChooseAlgorithmViewModel()
        {
            InitializeButtons();

            this.WindowTitle = "Wybierz algorytm";
        }

        public ChooseAlgorithmViewModel(string windowName)
        {
            InitializeButtons();

            Func<INotifyPropertyChanged, Type> typeLocator = (t) => App.GetViewClassTypeLocalizer(t);
            this._dialogService = new DialogService(null, typeLocator);

            this.WindowTitle = windowName + " - wybierz algorytm";
            this.ChooseAlgorithmButtons[0].IsEnabled = false;
        }

        // Custom graph mode
        public ChooseAlgorithmViewModel(Graph graph)
        {
            InitializeButtons();

            this.WindowTitle = "Tryb grafu niestandardowego - wybierz algorytm";
            this._graph = graph;

            Func<INotifyPropertyChanged, Type> typeLocator = (t) => App.GetViewClassTypeLocalizer(t);
            this._dialogService = new DialogService(null, typeLocator);

            for (int i = 0; i < graph.AlgorithmsSupported.Length; i++)
            {
                this.ChooseAlgorithmButtons[i].IsEnabled = graph.AlgorithmsSupported[i];
            }
        }

        private void InitializeButtons()
        {
            this.ChooseAlgorithmButtons.Add( new ChooseAlgorithmButton("Przeszukiwanie wszerz") );
            this.ChooseAlgorithmButtons.Add( new ChooseAlgorithmButton("Przeszukiwanie w głąb") );
            this.ChooseAlgorithmButtons.Add( new ChooseAlgorithmButton("Sortowanie topologiczne") );
            this.ChooseAlgorithmButtons.Add( new ChooseAlgorithmButton("Algorytm Kruskala") );
            this.ChooseAlgorithmButtons.Add( new ChooseAlgorithmButton("Wykrywanie dwudzielności") );
            this.ChooseAlgorithmButtons.Add( new ChooseAlgorithmButton("Algorytm Dijkstry") );
            this.ChooseAlgorithmButtons.Add( new ChooseAlgorithmButton("Algorytm Bellmana-Forda") );
        }

        //----------------------------------

        private string _windowTitle;
        private bool _isWindowVisible = true;

        private readonly IDialogService _dialogService;
        private readonly Graph _graph;

        //----------------------------------

        public string WindowTitle
        {
            get { return _windowTitle; }
            set
            {
                _windowTitle = value;
                RaisePropertyChanged("WindowTitle");
            }
        }
        public bool IsWindowVisible
        {
            get { return _isWindowVisible; }
            set
            {
                _isWindowVisible = value;
                RaisePropertyChanged("IsWindowVisible");
            }
        }

        public ObservableCollection<ChooseAlgorithmButton> ChooseAlgorithmButtons { get; set; } = new ObservableCollection<ChooseAlgorithmButton>();

        //----------------------------------
        
        private void ReturnClickExecute()
        {
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

            currentWindow.Close();
        }

        private void AlgorithmClickExecute(object[] parameters)
        {
            Window currentWindow = parameters[0] as Window;
            string algorithmName = parameters[1] as string;
            IModalDialogViewModel dialogViewModel = null;

            currentWindow.Visibility = Visibility.Hidden;

            if ( this.WindowTitle.StartsWith("Tryb nauki") )
            {
                dialogViewModel = new LearningViewModel(algorithmName);
            }
            else if ( this.WindowTitle.StartsWith("Tryb egzaminu") )
            {
                dialogViewModel = new LearningViewModel(algorithmName);               // ExamViewModel in future
            }
            else if ( this.WindowTitle.StartsWith("Tryb grafu niestandardowego") )
            {
                dialogViewModel = new CustomGraphViewModel(algorithmName, _graph);
            }

            _dialogService.ShowDialog(this, dialogViewModel);
            currentWindow.Close();
        }

        public ICommand AlgorithmClick
        {
            get { return new RelayCommand<object[]>( param => AlgorithmClickExecute(param) ); }
        }
        public ICommand ReturnClick
        {
            get { return new RelayCommand<object>( p => ReturnClickExecute() ); }
        }
    }
}
