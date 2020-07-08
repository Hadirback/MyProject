using System;
using System.Collections.Generic;
using System.Text;

namespace Slotlogic.MobileApp.Models.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private bool _isEnabled;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }

        private string _code;

        public string Code
        {
            get { return _code; }
            set
            {
                _code = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel() { }

        public MainViewModel(string code)
        {
            Code = code;
        }

        public MainViewModel(bool flag)
        {
            IsEnabled = flag;
        }
    }
}
