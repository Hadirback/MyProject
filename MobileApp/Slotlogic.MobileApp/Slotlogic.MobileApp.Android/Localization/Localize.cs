using Slotlogic.MobileApp.Localization;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using Xamarin.Forms;
[assembly: Dependency(typeof(LocalizeApp.Droid.Localize))]

namespace LocalizeApp.Droid
{
    public class Localize : ILocalize
    {
        public void SetLocale(CultureInfo ci)
        {
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
        }

        public System.Globalization.CultureInfo GetCurrentCultureInfo()
        {
            string netLanguage = "en-GB";
            var androidLocale = Java.Util.Locale.Default;
            netLanguage = AndroidToDotnetLanguage(androidLocale.ToString().Replace("_", "-"));
            
            System.Globalization.CultureInfo ci = null;
            try
            {
                ci = new System.Globalization.CultureInfo(netLanguage);
            }
            catch (CultureNotFoundException e1)
            {
                try
                {
                    string fallback = ToDotnetFallbackLanguage(new PlatformCulture(netLanguage));
                    ci = new System.Globalization.CultureInfo(fallback);
                    Debug.WriteLine(e1.Message);
                }
                catch (CultureNotFoundException e2)
                {
                    ci = new System.Globalization.CultureInfo("en-GB");
                    Debug.WriteLine(e2.Message);
                }
            }
            return ci;
        }
        string AndroidToDotnetLanguage(string androidLanguage)
        {
            string netLanguage = androidLanguage;
            //certain languages need to be converted to CultureInfo equivalent
            switch (androidLanguage)
            {
                case "en-US":   
                case "en-AU":   
                    netLanguage = "en-GB"; 
                    break;
            }
            return netLanguage;
        }

        string ToDotnetFallbackLanguage(PlatformCulture platCulture)
        {
            var netLanguage = platCulture.LanguageCode; // use the first part of the identifier (two chars, usually);
            switch (platCulture.LanguageCode)
            {
                case "gsw":
                    netLanguage = "de-CH"; // equivalent to German (Switzerland) for this app
                    break;
                    // add more application-specific cases here (if required)
                    // ONLY use cultures that have been tested and known to work
            }
            return netLanguage;
        }
    }
}