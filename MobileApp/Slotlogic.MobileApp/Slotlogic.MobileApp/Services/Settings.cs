using System;
using System.Collections.Generic;
using System.Text;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Slotlogic.MobileApp.Services
{
    public static class Settings
    {
        public static Dictionary<string, Dictionary<string, string>> Localization { get; set; }

        private static ISettings AppSettings
        {
            get { return CrossSettings.Current; }
        }

        public static string IpAddress
        {
            get { return "10.9.9.6"; }
        }

        public static string UrlBase
        {
            get { return $"http://{IpAddress}:8000/api/v{MainVersion}/"; }
        }

        public static string Version
        {
            get { return "1.1"; }
        }
        public static string MainVersion
        {
            get { return Version.Split('.')[0]; }
        }

        public static string ControllerDefaultName
        {
            get { return "MobileAppV"; }
        }

        public static string LogInSystem
        {
            get { return $"{ControllerDefaultName}{MainVersion}/LogInSystem/"; }
        }

        public static string AboutClubInfo
        {
            get { return $"{ControllerDefaultName}{MainVersion}/GetAboutClubInfo/"; }
        }

        public static string DrawsInfo
        {
            get { return $"{ControllerDefaultName}{MainVersion}/GetDrawsInfo"; }
        }

        public static string WriteError
        {
            get { return $"{ControllerDefaultName}{MainVersion}/WriteError"; }
        }

        public static string ResourcesDataPlayer
        {
            get { return $"{ControllerDefaultName}{MainVersion}/GetResourcesDataPlayer"; }
        }
    }
}
