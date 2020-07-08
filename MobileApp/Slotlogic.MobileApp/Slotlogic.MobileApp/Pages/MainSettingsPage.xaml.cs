using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slotlogic.MobileApp.Localization;
using Slotlogic.MobileApp.Models.InterfaceModel;
using Slotlogic.MobileApp.Models.View;
using Slotlogic.MobileApp.Models.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Slotlogic.MobileApp.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainSettingsPage : ContentPage
	{

        public MainSettingsPage ()
		{
			InitializeComponent ();
            this.BindingContext = new MainSettingsViewModel();
            this.Init();
        }

        void Init()
        {
            // Set Default Value for picker
            LanguageItem language = (BindingContext as MainSettingsViewModel).LanguageItems
                .FirstOrDefault(s => s.Code == App.Current.Properties["LocalizeApp"]
                .ToString());
            PickerSettingsLanguage.SelectedItem = language;
        }

        void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            // Get SelectedItem of picker by selectedIndex in MainSettingsViewModel class
            if (selectedIndex != -1)
            {
               (BindingContext as MainSettingsViewModel).LanguageSelectedIndex = selectedIndex;
            }

            // if localization not equals SelectedItem of picker then we take SelectedItem as main value
            if ((picker.SelectedItem as LanguageItem).Code != App.Current.Properties["LocalizeApp"].ToString())
            {
                LocalizeService.SetLocalizationByValue((picker.SelectedItem as LanguageItem).Code);
                Application.Current.MainPage = new MainSettingsPage();
            }
        }

        private void ButtonSettingsBack_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new MainPage();
        }
    }
}