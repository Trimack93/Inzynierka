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

namespace WpfApplication1.Views
{
    /// <summary>
    /// Interaction logic for ChooseAlgorithmView.xaml
    /// </summary>
    public partial class ChooseAlgorithmView : Window
    {
        private enum Mode { Learning, Exam }
        private Mode _currentMode;

        public ChooseAlgorithmView(string windowName)
        {
            InitializeComponent();

            this.Title = windowName + " - wybierz algorytm";

            if (windowName == "Tryb nauki")
                _currentMode = Mode.Learning;
            else
                _currentMode = Mode.Exam;
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AlgorithmButton_Click(object sender, RoutedEventArgs e)
        {
            Grid buttonContent = (sender as Button).Content as Grid;
            TextBlock buttonText = buttonContent.Children[1] as TextBlock;

            Window newWindow;
            string newWindowName = buttonText.Text;
            this.Hide();

            if (_currentMode == Mode.Learning)
                newWindow = new LearningView(newWindowName);
            else
                newWindow = new LearningView(newWindowName);                   // new ExamView() in future

            newWindow.ShowDialog();
            this.Close();
        }
    }
}
