using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Common.Models;
using Common.Utilities;

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
            string path = Path.GetFullPath("../../../GraphGenerator/CustomGraphs");

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Custom graph files (*.huu)|*.huu";
            dlg.InitialDirectory = path;
            
            if ( dlg.ShowDialog() == true )
            {
                try
                {
                    List<Graph> graphsList;

                    CustomXmlSerializer xmlSerializer = new CustomXmlSerializer(typeof(List<Graph>));

                    string encryptedXml = File.ReadAllText(dlg.FileName);
                    string plainXml = StringEncryption.Decrypt(encryptedXml, "Yaranaika?");

                    using (StringReader reader = new StringReader(plainXml))
                    {
                        graphsList = (List<Graph>)xmlSerializer.Deserialize(reader);
                    }

                    this.Hide();

                    ChooseAlgorithmView algorithmView = new ChooseAlgorithmView(graphsList[0]);               // First graph is always user graph
                    algorithmView.ShowDialog();

                    this.Show();
                }
                catch (Exception)
                {
                    MessageBox.Show("Wystąpił nieoczekiwny błąd.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            AboutView view = new AboutView();
            view.ShowDialog();
        }
    }
}
