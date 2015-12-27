using Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class ComboboxElement : BaseNotifyPropertyChanged
    {
        public ComboboxElement(short ID, bool isValueChosen = false)
        {
            this.ID = ID;
            this.IsValueChosen = IsValueChosen;
        }

        //---------------------------------

        private short _ID;
        private bool _isValueChosen;

        //---------------------------------

        public short ID
        {
            get { return _ID; }
            set
            {
                _ID = value;
                RaisePropertyChanged("ID");
            }
        }
        public bool IsValueChosen
        {
            get { return _isValueChosen; }
            set
            {
                _isValueChosen = value;
                RaisePropertyChanged("IsValueChosen");
            }
        }
    }
}
