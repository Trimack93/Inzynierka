﻿using System;
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

namespace GraphGenerator.Views
{
    /// <summary>
    /// Interaction logic for AddNodeView.xaml
    /// </summary>
    public partial class AddNodeView : Window
    {
        public AddNodeView()
        {
            InitializeComponent();
        }

        //private void TextBox_KeyDown(object sender, KeyEventArgs e)
        //{
        //    TextBox t = sender as TextBox;

        //    if (t.Text.Length > 2)
        //        e.Handled = true;
        //}
    }
}
