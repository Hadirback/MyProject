using Slotlogic.MobileApp.Models.Common;
using Slotlogic.MobileApp.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Slotlogic.MobileApp.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DrawsPage : ContentPage
	{
		public DrawsPage () {} 

        public DrawsPage(List<DrawsInfo> drawsList)
        {
            InitializeComponent();
            this.BindingContext = new DrawsViewModel(drawsList, this);
        }
    }
}