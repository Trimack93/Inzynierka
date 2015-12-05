using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Utilities;
using Common.Models;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.ObjectModel;
using MvvmDialogs;

namespace GraphGenerator.ViewModels
{
    public class AddNodeViewModel : BaseNotifyPropertyChanged, IModalDialogViewModel
    {
        public AddNodeViewModel()
        {
            Node.Name = "a";
            Node.Value = 0;
            Node.NameHorizontalAlignment = HorizontalAlignment.Center;
            Node.NameVerticalAlignment = VerticalAlignment.Top;

            ButtonColors.Add(Brushes.LimeGreen);
            ButtonColors.Add(Brushes.LightGray);
            ButtonColors.Add(Brushes.LightGray);
            ButtonColors.Add(Brushes.LightGray);
        }

        //----------------------------------

        private Node _Node = new Node();
        private bool? dialogResult = false;
        private ObservableCollection<SolidColorBrush> _ButtonColors = new ObservableCollection<SolidColorBrush>();

        //----------------------------------

        public Node Node
        {
            get { return _Node; }
            set
            {
                _Node = value;
                RaisePropertyChanged("Node");
            }
        }
        public bool? DialogResult
        {
            get { return dialogResult; }
            private set
            {
                dialogResult = value;
                RaisePropertyChanged("DialogResult");
            }
        }
        public ObservableCollection<SolidColorBrush> ButtonColors
        {
            get { return _ButtonColors; }
            private set { _ButtonColors = value; }
        }

        //----------------------------------

        void SelectTopCenterAlignmentExecute()
        {
            Node.NameVerticalAlignment = VerticalAlignment.Top;
            Node.NameHorizontalAlignment = HorizontalAlignment.Center;

            ButtonColors[0] = Brushes.LimeGreen;
            ButtonColors[1] = Brushes.LightGray;
            ButtonColors[2] = Brushes.LightGray;
            ButtonColors[3] = Brushes.LightGray;
        }
        void SelectCenterLeftAlignmentExecute()
        {
            if (Node.Name.Length < 3)
            {
                Node.NameVerticalAlignment = VerticalAlignment.Center;
                Node.NameHorizontalAlignment = HorizontalAlignment.Left;

                ButtonColors[0] = Brushes.LightGray;
                ButtonColors[1] = Brushes.LimeGreen;
                ButtonColors[2] = Brushes.LightGray;
                ButtonColors[3] = Brushes.LightGray;
            }
        }
        void SelectCenterRightAlignmentExecute()
        {
            if (Node.Name.Length < 3)
            {
                Node.NameVerticalAlignment = VerticalAlignment.Center;
                Node.NameHorizontalAlignment = HorizontalAlignment.Right;

                ButtonColors[0] = Brushes.LightGray;
                ButtonColors[1] = Brushes.LightGray;
                ButtonColors[2] = Brushes.LimeGreen;
                ButtonColors[3] = Brushes.LightGray;
            }
        }
        void SelectBottomCenterAlignmentExecute()
        {
            Node.NameVerticalAlignment = VerticalAlignment.Bottom;
            Node.NameHorizontalAlignment = HorizontalAlignment.Center;

            ButtonColors[0] = Brushes.LightGray;
            ButtonColors[1] = Brushes.LightGray;
            ButtonColors[2] = Brushes.LightGray;
            ButtonColors[3] = Brushes.LimeGreen;
        }
        void OkClickExecute()
        {
            this.DialogResult = true;
        }

        //----------------------------------

        public ICommand SelectTopCenterAlignment
        {
            get { return new RelayCommand<object>(p => SelectTopCenterAlignmentExecute()); }
        }
        public ICommand SelectCenterLeftAlignment
        {
            get { return new RelayCommand<object>(p => SelectCenterLeftAlignmentExecute()); }
        }
        public ICommand SelectCenterRightAlignment
        {
            get { return new RelayCommand<object>(p => SelectCenterRightAlignmentExecute()); }
        }
        public ICommand SelectBottomCenterAlignment
        {
            get { return new RelayCommand<object>(p => SelectBottomCenterAlignmentExecute()); }
        }
        public ICommand OkClick
        {
            get { return new RelayCommand<object>(p => OkClickExecute()); }
        }
    }
}
