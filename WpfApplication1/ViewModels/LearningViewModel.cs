using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Utilities;
using System.Collections.ObjectModel;
using Common.Models.Canvas;
using System.Windows;
using Common.Models;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;
using WpfApplication1.Models.Algorithms;
using System.IO;

namespace WpfApplication1.ViewModels
{
    public class LearningViewModel : BaseNotifyPropertyChanged
    {
        public LearningViewModel()
        {
            this.AlgorithmName = "[Nazwa algorytmu]";
        }

        public LearningViewModel(string algorithmName)
        {
            this.AlgorithmName = algorithmName;

            //int columnsCount = this.CanvasWidth / this.RectangleSize;
            //int rowsCount = this.CanvasHeight / this.RectangleSize;

            //for (int i = 0; i < rowsCount; i++)
            //    for (int j = 0; j < columnsCount; j++)
            //    {
            //        CanvasItems.Add(new CanvasRectangle(i * columnsCount + j, j * this.RectangleSize, i * this.RectangleSize, this.RectangleSize));
            //    }

            //(CanvasItems[1] as CanvasRectangle).Node = new Node()
            //{ ID = 0, Name = "a", Value = 6, NameHorizontalAlignment = HorizontalAlignment.Center, NameVerticalAlignment = VerticalAlignment.Top };

            //(CanvasItems[69] as CanvasRectangle).Node = new Node()
            //{ ID = 1, Name = "b", Value = 9, NameHorizontalAlignment = HorizontalAlignment.Right, NameVerticalAlignment = VerticalAlignment.Center };

            //(CanvasItems[24] as CanvasRectangle).Node = new Node()
            //{ ID = 2, Name = "c", Value = 15, NameHorizontalAlignment = HorizontalAlignment.Left, NameVerticalAlignment = VerticalAlignment.Center };

            //(CanvasItems[1] as CanvasRectangle).DoesContainNode = true;
            //(CanvasItems[69] as CanvasRectangle).DoesContainNode = true;
            //(CanvasItems[24] as CanvasRectangle).DoesContainNode = true;

            List<Graph> graphsList;

            CustomXmlSerializer xmlSerializer = new CustomXmlSerializer( typeof(List<Graph>) );
            
            string encryptedXml = File.ReadAllText(GRAPH_PATH);
            string plainXml = StringEncryption.Decrypt(encryptedXml, "Yaranaika?");

            using ( StringReader reader = new StringReader(plainXml) )
            {
                graphsList = ( List<Graph> )xmlSerializer.Deserialize(reader);
            }

            switch (this.AlgorithmName)
            {
                case "Przeszukiwanie wszerz":
                    _algorithm = new BFS( CanvasItems.GetAllEdges(), CanvasItems.GetAllNodes() );

                    this.CanvasItems = GetRandomGraphFromList(graphsList, 0);
                    break;
            }

            this.Instruction = _algorithm?.Instructions[0];

            RaisePropertyChanged("CanvasNodes");
        }

        //----------------------------------

        private bool _isStopButtonVisible { get; set; } = false;
        private bool _isNodeNamesControlVisible { get; set; } = true;
        private bool _isNodeNamesControlEnabled { get; set; } = true;

        private readonly string GRAPH_PATH = Path.GetFullPath("../../Resources/Graphs/EncryptedData.huu");
        private string _instruction;

        private ObservableCollection<CanvasControlBase> _canvasItems = new ObservableCollection<CanvasControlBase>();

        private AlgorithmBase _algorithm { get; set; }

        //----------------------------------

        public bool IsStopButtonVisible
        {
            get { return _isStopButtonVisible; }
            set
            {
                _isStopButtonVisible = value;
                RaisePropertyChanged("IsStopButtonVisible");
                RaisePropertyChanged("IsAdditionalControlVisible");
            }
        }
        public bool IsNodeNamesControlVisible
        {
            get { return _isNodeNamesControlVisible; }
            set
            {
                _isNodeNamesControlVisible = value;
                RaisePropertyChanged("IsNodeNamesControlVisible");
                RaisePropertyChanged("IsAdditionalControlVisible");
            }
        }
        public bool IsNodeNamesControlEnabled
        {
            get { return _isNodeNamesControlEnabled; }
            set
            {
                _isNodeNamesControlEnabled = value;
                RaisePropertyChanged("IsNodeNamesControlEnabled");
            }
        }
        public bool IsAdditionalControlVisible
        {
            get { return IsStopButtonVisible || IsNodeNamesControlVisible; }
        }

        public ObservableCollection<CanvasControlBase> CanvasItems
        {
            get { return _canvasItems; }
            private set
            {
                _canvasItems = value;

                RaisePropertyChanged("CanvasItems");
                RaisePropertyChanged("CanvasNodes");
            }
        }
        public List<Node> CanvasNodes
        {
            get { return CanvasItems.GetAllNodes(); }
        }

        public int CanvasWidth { get { return 726; } }
        public int CanvasHeight { get { return 660; } }
        public int RectangleSize { get { return 66; } }

        public string AlgorithmName { get; private set; }
        public string Instruction
        {
            get { return _instruction; }
            set
            {
                _instruction = value;
                RaisePropertyChanged("Instruction");
            }
        }
        
        //public string Instruction { get; set; } = @"Instrukcja dla aktualnie wykonywanej sekwencji. asdf asdf
        //                           Test nowej linii
        //                           fdsgsdfg";                                   // move to model of each algorithm

        //----------------------------------

        private ObservableCollection<CanvasControlBase> GetRandomGraphFromList(List<Graph> graphsList, byte algorithmID)
        {
            List<Graph> compatibileGraphsList = graphsList
                .Where(g => g.AlgorithmsSupported[algorithmID] == true)
                .ToList();

            int randomIndex = new Random().Next(compatibileGraphsList.Count);

            return compatibileGraphsList[randomIndex].CanvasGraph;
        }

        //----------------------------------

        void EndSequenceClickExecute()
        {
            IsNodeNamesControlEnabled = !IsNodeNamesControlEnabled;
        }

        void ContextMenuOpenedExecute(RoutedEventArgs e)
        {
            ContextMenu contextMenu = e.Source as ContextMenu;                      // change to custom control in future - temporary workaround

            contextMenu.IsOpen = true;
        }
        void MarkNodeBlackExecute(int nodeID)
        {
            List<Node> nodesList = CanvasItems.GetAllNodes();
            nodesList.Single(n => n.ID == nodeID).Color = Brushes.Black;

            MessageBox.Show("Corny nod xD");
        }

        //----------------------------------

        public ICommand EndSequenceClick
        {
            get { return new RelayCommand<object>(p => EndSequenceClickExecute()); }
        }
        public ICommand ContextMenuOpened
        {
            get { return new RelayCommand<RoutedEventArgs>( p => ContextMenuOpenedExecute(p) ); }
        }
        public ICommand MarkNodeBlack
        {
            get { return new RelayCommand<int>( param => MarkNodeBlackExecute(param) ); }
        }
    }
}
