using Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    // May be updated in future with new properties
    public class ComboboxElement : BaseNotifyPropertyChanged
    {
        public ComboboxElement() { }

        public ComboboxElement(int ID)
        {
            this.ID = ID;
        }

        //---------------------------------

        private int _ID;
        private Node _selectedValue;

        //---------------------------------

        public int ID
        {
            get { return _ID; }
            set
            {
                _ID = value;
                RaisePropertyChanged("ID");
            }
        }
        public Node SelectedValue
        {
            get { return _selectedValue; }
            set
            {
                _selectedValue = value;
                RaisePropertyChanged("SelectedValue");
            }
        }

        //---------------------------------

        /// <summary>
        /// Creates deep copy of current object.
        /// </summary>
        public ComboboxElement Clone()
        {
            ComboboxElement newElement = new ComboboxElement();
            newElement.ID = this.ID;
            newElement.SelectedValue = this.SelectedValue?.Clone();

            return newElement;
        }
    }
}
