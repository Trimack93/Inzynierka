using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
            string path = Path.GetFullPath("../../Resources/Graphs");

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Custom graph files (*.huu)|*.huu";
            dlg.InitialDirectory = path;
            
            bool? result = dlg.ShowDialog();

            MessageBox.Show("Czy plik został otwarty: " + result.ToString());
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            AboutView view = new AboutView();
            view.ShowDialog();
        }
    }
}
