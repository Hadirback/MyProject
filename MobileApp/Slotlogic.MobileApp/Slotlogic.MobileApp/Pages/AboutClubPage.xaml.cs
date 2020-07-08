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
	public partial class AboutClubPage : ContentPage
	{
		public AboutClubPage (){}

        public AboutClubPage(string aboutClub)
        {
            InitializeComponent();
            //App.StartCheckIfInternet(LabelNoInternet, this);
            this.BindingContext = new AboutClubViewModel(aboutClub);
        }
    }
}