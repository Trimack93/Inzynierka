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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication1.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LearningButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();

            ChooseAlgorithmView algorithmView = new ChooseAlgorithmView("Tryb nauki");
            algorithmView.ShowDialog();

            this.Show();
        }

        private void ExamButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();

            ChooseAlgorithmView algorithmView = new ChooseAlgorithmView("Tryb egzaminu");
            algorithmView.ShowDialog();

            this.Show();
        }

        private void CustomGraphButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Wait for it.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            AboutView view = new AboutView();
            view.ShowDialog();
        }
    }
}
