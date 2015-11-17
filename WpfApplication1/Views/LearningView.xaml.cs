using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using WpfApplication1.ViewModels;

namespace WpfApplication1.Views
{
    /// <summary>
    /// Interaction logic for LearningView.xaml
    /// </summary>
    public partial class LearningView : Window
    {
        private LearningViewModel _viewModel;
        public LearningView()
        {
            InitializeComponent();
        }

        public LearningView(string algorithmName)
        {
            InitializeComponent();

            _viewModel = new LearningViewModel(algorithmName);
            base.DataContext = _viewModel;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            AboutView view = new AboutView();
            view.ShowDialog();
        }
    }
}
