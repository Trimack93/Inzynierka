using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GraphGenerator.ViewModels
{
    public class EditNodeViewModel : AddNodeViewModel
    {
        public EditNodeViewModel() : base() { }

        public EditNodeViewModel(Node nodeToEdit)
        {
            this.Node.Name = nodeToEdit.Name;
            this.Node.Value = nodeToEdit.Value;
            this.Node.NameHorizontalAlignment = nodeToEdit.NameHorizontalAlignment;
            this.Node.NameVerticalAlignment = nodeToEdit.NameVerticalAlignment;

            if ( nodeToEdit.NameVerticalAlignment == VerticalAlignment.Top && nodeToEdit.NameHorizontalAlignment == HorizontalAlignment.Center)
                SelectTopCenterAlignmentExecute();
            else if (nodeToEdit.NameVerticalAlignment == VerticalAlignment.Center && nodeToEdit.NameHorizontalAlignment == HorizontalAlignment.Left)
                SelectCenterLeftAlignmentExecute();
            else if (nodeToEdit.NameVerticalAlignment == VerticalAlignment.Center && nodeToEdit.NameHorizontalAlignment == HorizontalAlignment.Right)
                SelectCenterRightAlignmentExecute();
            else if (nodeToEdit.NameVerticalAlignment == VerticalAlignment.Bottom && nodeToEdit.NameHorizontalAlignment == HorizontalAlignment.Center)
                SelectBottomCenterAlignmentExecute();

        }
    }
}
