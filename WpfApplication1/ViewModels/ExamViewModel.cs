using Common.Models;
using Common.Utilities;
using MvvmDialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Common.Models.Canvas;
using System.Collections.ObjectModel;
using WpfApplication1.Models.Algorithms;
using WpfApplication1.Models;
using System.Windows.Media;
using System.Windows.Input;

namespace WpfApplication1.ViewModels
{
    public class ExamViewModel : BaseViewModel
    {
        public ExamViewModel() : base("../../Resources/Graphs/ExamData.huu")
        {
            this.AlgorithmName = "[Nazwa algorytmu]";
            this.IsExamPanelVisible = true;
            this.PageColors.Add(Brushes.Yellow);
            this.PageColors.Add(Brushes.Yellow);
        }

        public ExamViewModel(string algorithmName) : base("../../Resources/Graphs/ExamData.huu")
        {
            this.IsExamPanelVisible = true;
            this.PageColors.Add(Brushes.Yellow);
            this.PageColors.Add(Brushes.Yellow);

            try
            {
                Func<INotifyPropertyChanged, Type> typeLocator = (t) => App.GetViewClassTypeLocalizer(t);
                this._dialogService = new DialogService(null, typeLocator);

                this.AlgorithmName = algorithmName;
                this.LoadGraphs = GetGraphsFromList;

                List<Graph> graphsList;

                CustomXmlSerializer xmlSerializer = new CustomXmlSerializer(typeof(List<Graph>));

                string encryptedXml = File.ReadAllText(GRAPH_PATH);
                string plainXml = StringEncryption.Decrypt(encryptedXml, "Yaranaika?");

                using (StringReader reader = new StringReader(plainXml))
                {
                    graphsList = (List<Graph>)xmlSerializer.Deserialize(reader);
                }

                InitializeAlgorithmFromGraph(graphsList);
                InitializeAlgorithms();

                RaisePropertyChanged("CanvasNodes");
            }
            catch (Exception)
            {
                MessageBox.Show("Wystąpil nieoczekiwany błąd. Upewnij się, że pliki danych nie są uszkodzone.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);

                this.DialogResult = false;
            }
        }

        //----------------------------------

        private ObservableCollection<SolidColorBrush> _pageColors = new ObservableCollection<SolidColorBrush>();

        private int _currentPageIndex { get; set; }

        private AlgorithmBase _firstPageAlgorithm { get; set; }
        private AlgorithmBase _secondPageAlgorithm { get; set; }
        private ObservableCollection<ComboboxElement> _firstPageNodesList { get; set; } = new ObservableCollection<ComboboxElement>();
        private ObservableCollection<ComboboxElement> _secondPageNodesList { get; set; } = new ObservableCollection<ComboboxElement>();

        //----------------------------------

        public ObservableCollection<SolidColorBrush> PageColors
        {
            get { return _pageColors; }
            set
            {
                _pageColors = value;
                RaisePropertyChanged("PageColors");
            }
        }
        public List<ExamPage> ExamPages { get; set; } = new List<ExamPage>(2);

        //----------------------------------

        private ObservableCollection<CanvasControlBase> GetGraphsFromList(List<Graph> graphsList, byte algorithmID)
        {
            List<Graph> compatibileGraphsList = graphsList
                .Where(g => g.AlgorithmsSupported[algorithmID] == true)
                .ToList();

            Random rand = new Random();

            int firstGraphIndex = rand.Next(compatibileGraphsList.Count);
            int secondGraphIndex;

            do
            {
                secondGraphIndex = rand.Next(compatibileGraphsList.Count);
            } while (secondGraphIndex == firstGraphIndex);

            ExamPage page1 = new ExamPage(compatibileGraphsList[firstGraphIndex].CanvasGraph);
            ExamPage page2 = new ExamPage(compatibileGraphsList[secondGraphIndex].CanvasGraph);

            ExamPages.Add(page1);
            ExamPages.Add(page2);
            this._currentPageIndex = 0;

            return ExamPages[0].Graph;
        }

        private void InitializeAlgorithms()
        {
            _firstPageAlgorithm = _algorithm;
            ExamPage secondPage = ExamPages[1];

            ObservableCollection<ComboboxElement> queue = new ObservableCollection<ComboboxElement>();

            switch (this.AlgorithmName)
            {
                case "Przeszukiwanie wszerz":
                    queue.Add( new ComboboxElement(0) );

                    _secondPageAlgorithm = new BFS(secondPage.Graph.GetAllEdges(), secondPage.Graph.GetAllNodes(), queue);
                    break;

                case "Przeszukiwanie w głąb":
                    _secondPageAlgorithm = new DFS(secondPage.Graph.GetAllEdges(), secondPage.Graph.GetAllNodes());
                    break;

                case "Sortowanie topologiczne":
                    _secondPageAlgorithm = new TopologicalSort(secondPage.Graph.GetAllEdges(), secondPage.Graph.GetAllNodes(), queue);
                    break;

                case "Algorytm Kruskala":
                    _secondPageAlgorithm = new Kruskal(secondPage.Graph.GetAllEdges(), secondPage.Graph.GetAllNodes());
                    break;

                case "Wykrywanie dwudzielności":
                    queue.Add(new ComboboxElement(0));

                    _secondPageAlgorithm = new Bipartition(secondPage.Graph.GetAllEdges(), secondPage.Graph.GetAllNodes(), queue);
                    break;

                case "Algorytm Dijkstry":
                    _secondPageAlgorithm = new Dijkstra(secondPage.Graph.GetAllEdges(), secondPage.Graph.GetAllNodes());
                    break;

                case "Algorytm Bellmana-Forda":
                    queue.Add(new ComboboxElement(0));

                    _secondPageAlgorithm = new BellmanFord(secondPage.Graph.GetAllEdges(), secondPage.Graph.GetAllNodes(), queue);
                    break;
            }
        }

        private void FailStudentAndClose(Window examWindow)
        {
            MessageBox.Show("Popełniłeś błąd! Do zobaczenia.", "Caban rzecze", MessageBoxButton.OK, MessageBoxImage.Information);

            if (examWindow != null)
                examWindow.Close();
        }

        private void FinishAlgorithm(Window examWindow)
        {
            ExamPages[_currentPageIndex].IsPassed = true;
            PageColors[_currentPageIndex] = Brushes.Green;

            if ( ExamPages.All(e => e.IsPassed) )
            {
                MessageBox.Show("Gratulacje, zdałeś egzamin!\nZa chwilę nastąpi przejście do głównego menu...", "Informacja",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                if (examWindow != null)
                    examWindow.Close();
            }
            else
            {
                MessageBox.Show("Zadanie zaliczone.", "Informacja",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                ChangePage( ExamPages.First(p => p.IsPassed == false).Graph );
            }
        }

        private void ChangePage(ObservableCollection<CanvasControlBase> graph)
        {
            if (_currentPageIndex == 0)
            {
                _currentPageIndex = 1;
                _algorithm = _secondPageAlgorithm;

                if (this.ComboBoxItems != null)
                {
                    _firstPageNodesList.Clear();
                    foreach (ComboboxElement elem in this.ComboBoxItems)
                    {
                        _firstPageNodesList.Add(elem.Clone());
                    }
                }
            }
            else
            {
                _currentPageIndex = 0;
                _algorithm = _firstPageAlgorithm;

                if (this.ComboBoxItems != null)
                {
                    _secondPageNodesList.Clear();
                    foreach (ComboboxElement elem in this.ComboBoxItems)
                    {
                        _secondPageNodesList.Add(elem.Clone());
                    }
                }
            }

            this.CanvasItems = graph;

            this.IsNodeNamesControlEnabled = false;
            this.IsNodeNamesControlVisible = false;

            if (_algorithm is IAlgorithmWithQueue)
            {
                this.ComboBoxItems = (_algorithm as IAlgorithmWithQueue).NodesQueue;
                List<Node> nodesList = graph.GetAllNodes();

                if (_currentPageIndex == 0)
                {
                    for (int i = 0; i < _firstPageNodesList.Count; i++)
                    {
                        Node realNode = nodesList.SingleOrDefault(n => n.ID == _firstPageNodesList[i].SelectedValue?.ID);

                        this.ComboBoxItems[i].SelectedValue = realNode;
                    }
                }
                else
                {
                    for (int i = 0; i < _secondPageNodesList.Count; i++)
                    {
                        Node realNode = nodesList.SingleOrDefault(n => n.ID == _secondPageNodesList[i].SelectedValue?.ID);

                        this.ComboBoxItems[i].SelectedValue = realNode;
                    }
                }

                if (_algorithm is BFS)
                {
                    this.IsNodeNamesControlEnabled = true;
                    this.IsNodeNamesControlVisible = true;
                }

                if (_algorithm is TopologicalSort && (_algorithm as TopologicalSort).isSortingNodes)
                {
                    this.IsNodeNamesControlEnabled = true;
                    this.IsNodeNamesControlVisible = true;
                }

                if (_algorithm is BellmanFord)
                {
                    this.IsNodeNamesControlVisible = true;

                    if ((_algorithm as BellmanFord).NodesQueue.Where(n => n.SelectedValue != null).Count() == _algorithm.CorrectNodesList.Count)
                        this.IsNodeNamesControlEnabled = false;
                    else
                        this.IsNodeNamesControlEnabled = true;

                    if ((_algorithm as BellmanFord).IsLastStep)
                        this.IsStopButtonVisible = true;
                }
            }
        }

        //----------------------------------

        protected override void EndSequenceClickExecute(Window examWindow)
        {
            bool isSequenceGood = _algorithm.Step();

            if (isSequenceGood)
            {
                if (_algorithm is TopologicalSort && (_algorithm as TopologicalSort).isSortingNodes)
                {
                    this.IsNodeNamesControlEnabled = true;
                    this.IsNodeNamesControlVisible = true;
                }

                if (_algorithm is BellmanFord)
                {
                    this.IsNodeNamesControlEnabled = false;

                    if ((_algorithm as BellmanFord).IsLastStep)
                        this.IsStopButtonVisible = true;
                }

                if (_algorithm.IsFinished)
                {
                    if (_algorithm is Bipartition)
                    {
                        DivideNodesViewModel viewModel = new DivideNodesViewModel(_algorithm.CorrectNodesList, false);
                        bool? result = _dialogService.ShowDialog(this, viewModel);

                        if (result == false)
                        {
                            FailStudentAndClose(examWindow);
                            return;
                        }
                    }

                    FinishAlgorithm(examWindow);
                }
            }
            else
            {
                FailStudentAndClose(examWindow);
            }
        }

        protected override void StopButtonClickExecute(Window examWindow)
        {
            if (_algorithm is Bipartition)
            {
                if ( (_algorithm as Bipartition).AreNeighboursWithDifferentParity() == false )
                {
                    FinishAlgorithm(examWindow);
                }
                else
                {
                    FailStudentAndClose(examWindow);
                }
            }

            else if (_algorithm is BellmanFord)
            {
                if ( (_algorithm as BellmanFord).IsGraphWithoutNegativeCycle() == false )
                {
                    FinishAlgorithm(examWindow);
                }
                else
                {
                    FailStudentAndClose(examWindow);
                }
            }
        }

        private void PageClickExecute(string pageNumberAsStringStupidWPF)
        {
            int pageIndex = Int32.Parse(pageNumberAsStringStupidWPF) - 1;

            if (pageIndex != _currentPageIndex)
            {
                if (ExamPages[pageIndex].IsPassed == false)
                    ChangePage(ExamPages[pageIndex].Graph);
                else
                    MessageBox.Show("Zadanie zostało już oddane.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        //----------------------------------

        public ICommand PageClick
        {
            get { return new RelayCommand<string>(param => PageClickExecute(param)); }
        }

        //----------------------------------

        public string ExamInfo { get; set; } = @"Próg zaliczeniowy: 100%

        Kolejne zadania dostępne są po kliknięciu w odpowiedni numer.

        W trakcie egzaminu można przeglądać wszystkie niezaliczone jeszcze zadania.

        Po zaliczeniu zadania zostaniesz automatycznie przeniesiony do następnego. Po rozwiązaniu wszystkich zadań egzamin zakończy się.";
    }
}
