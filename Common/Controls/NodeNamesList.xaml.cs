using Common.Models;
using Common.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Common.Controls
{
    /// <summary>
    /// Interaction logic for NodeNamesList.xaml
    /// </summary>
    public partial class NodeNamesList : UserControl
    {
        public NodeNamesList()
        {
            InitializeComponent();

            //ElementsList = new ObservableCollection<ComboboxElement>();
            //ElementsList.Add(new ComboboxElement(0));
        }

        //----------------------------------

        public int ElementWidth
        {
            get { return (int)GetValue(ElementWidthProperty); }
            set { SetValue(ElementWidthProperty, value); }
        }
        public bool ComboBoxEnabled
        {
            get { return (bool)GetValue(ComboBoxEnabledProperty); }
            set { SetValue(ComboBoxEnabledProperty, value); }
        }
        public List<Node> NodesList
        {
            get { return (List<Node>)GetValue(NodesListProperty); }
            set { SetValue(NodesListProperty, value); }
        }
        public ObservableCollection<ComboboxElement> ElementsList
        {
            get { return (ObservableCollection<ComboboxElement>)GetValue(ElementsListProperty); }
            set { SetValue(ElementsListProperty, value); }
        }

        public int ElementHeight
        {
            get { return ElementWidth - 10; }
        }

        //----------------------------------

        public static readonly DependencyProperty ElementWidthProperty = DependencyProperty.Register(
            "ElementWidth", typeof(int), typeof(NodeNamesList), new UIPropertyMetadata(null));

        public static readonly DependencyProperty ComboBoxEnabledProperty = DependencyProperty.Register(
            "ComboBoxEnabled", typeof(bool), typeof(NodeNamesList), new UIPropertyMetadata(null));

        public static readonly DependencyProperty NodesListProperty = DependencyProperty.Register(
            "NodesList", typeof( List<Node> ), typeof(NodeNamesList), new UIPropertyMetadata(null));

        public static readonly DependencyProperty ElementsListProperty = DependencyProperty.Register(
            "ElementsList", typeof( ObservableCollection<ComboboxElement> ), typeof(NodeNamesList), new FrameworkPropertyMetadata(null));

        //----------------------------------

        private static void OnElementsListChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var x = sender as NodeNamesList;

            var y = x.NodeItemsControl.Items.SourceCollection;
        }

        //----------------------------------

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Change of value from "" to something
            if (e.RemovedItems.Count == 0)
            {
                short newIndex = (short) ElementsList.Count;

                // if user changed value of the not-last element in the list
                if (ElementsList.Count < NodesList.Count)
                    ElementsList.Add( new ComboboxElement(newIndex) );

                ElementsList[newIndex - 1].Value = (e.AddedItems[0] as Node).Name;
                ElementsList[newIndex - 1].IsValueChosen = true;
            }
            else
            {
                Node newNode = null;
                Node oldNode = null;

                if (e.AddedItems.Count > 0 && e.RemovedItems.Count > 0)
                {
                    newNode = e.AddedItems[0] as Node;
                    oldNode = e.RemovedItems[0] as Node;
                }

                ComboboxElement changedElement = ElementsList
                    .Single(el => el.Value == oldNode?.Name);

                changedElement.Value = newNode?.Name;
            }
        }

        //----------------------------------

        void ContextMenuItemDeleteExecute(ComboboxElement element)
        {
            // If the list is not empty and last element has value
            if (ElementsList.Count > 1 && element.IsValueChosen)
            {
                ElementsList.Remove(element);

                // If the list was full and item was deleted, the last element must be empty to allow user to add new node.
                // However, if the last item was already empty, nothing will happen.
                if (ElementsList.Count <= NodesList.Count - 1 && ElementsList.Last().IsValueChosen)
                {
                    ElementsList.Add(new ComboboxElement( (short)ElementsList.Count ));
                }
            }
        }

        public ICommand ContextMenuItemDelete
        {
            get { return new RelayCommand<ComboboxElement>(p => ContextMenuItemDeleteExecute(p)); }
        }
    }
}
