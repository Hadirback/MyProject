using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Slotlogic.MobileApp.Models.View
{
    public class CustomPickerView : Picker
    {
        public static readonly BindableProperty ImageProperty = BindableProperty.Create(nameof(Icon), typeof(string), typeof(CustomPickerView), string.Empty);
        public string Icon
        {
            get
            {
                return (string)GetValue(ImageProperty);
            }
            set
            {
                SetValue(ImageProperty, value);
            }
        }
    }
}
