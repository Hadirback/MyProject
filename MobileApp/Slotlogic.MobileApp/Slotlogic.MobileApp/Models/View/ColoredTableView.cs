using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace Slotlogic.MobileApp.Models.View
{
    public partial class ColoredTableView : TableView
    {
        public static BindableProperty GroupHeaderColorProperty = BindableProperty.Create("GroupHeaderColor", typeof(Color), typeof(ColoredTableView), Color.White);
        public Color GroupHeaderColor
        {
            get
            {
                return (Color)GetValue(GroupHeaderColorProperty);
            }
            set
            {
                SetValue(GroupHeaderColorProperty, value);
            }
        }


    }
}