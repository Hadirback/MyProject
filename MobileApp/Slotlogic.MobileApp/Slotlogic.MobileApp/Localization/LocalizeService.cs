using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Slotlogic.MobileApp.Models.ViewModel;
using Slotlogic.MobileApp.Services;
using Xamarin.Forms;

namespace Slotlogic.MobileApp.Localization
{
    public static class LocalizeService
    {
        public static void SetLocalizationByValue(string code)
        {
            try
            {
                if (string.IsNullOrEmpty(code))
                    code = "en_GB";   

                CultureInfo ci = SetLocalizationInProperty(code);
                Localization.Resources.Culture = ci;
                DependencyService.Get<ILocalize>().SetLocale(ci);
            }
            catch (Exception exc)
            {
                Service.WriteToLog($"Error in SetLocalizeByValue function. Input param: {code}", exc);
            }
        }

        public static void SetLocalization()
        {
            try
            {
                CultureInfo ci = null;
                string localizeApp = App.Current.Properties.ContainsKey("LocalizeApp") ? App.Current.Properties["LocalizeApp"].ToString() : string.Empty;
                MainSettingsViewModel mainSettings = new MainSettingsViewModel();

                if (!string.IsNullOrEmpty(localizeApp) && mainSettings.LanguageItems.Select(s => s.Code).Contains(localizeApp))
                    ci = new System.Globalization.CultureInfo(App.Current.Properties["LocalizeApp"].ToString());

                if (ci == null)
                    ci = SetLocalizationInProperty();

                Localization.Resources.Culture = ci;
                DependencyService.Get<ILocalize>().SetLocale(ci);
            }
            catch (Exception exc)
            {
                Service.WriteToLog($"Error in SetLocalizeByValue SetLocalization.", exc);
            }
        }

        public static CultureInfo SetLocalizationInProperty(string code = null)
        {
            CultureInfo ci = code == null ? DependencyService.Get<ILocalize>().GetCurrentCultureInfo() 
                                        : new System.Globalization.CultureInfo(code); 

            App.Current.Properties.Remove("LocalizeApp");
            App.Current.Properties.Add("LocalizeApp", ci.Name);
            App.Current.SavePropertiesAsync();
            return ci;
        }
    }
}
