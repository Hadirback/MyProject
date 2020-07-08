using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading;
using Slotlogic.MobileApp.Models;
using System.Threading.Tasks;
using Slotlogic.MobileApp.Localization;
using Slotlogic.MobileApp.Services;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Slotlogic.MobileApp
{
    public partial class App : Application
    {
        static RestService restService;

        private static Label labelScreen;
        private static bool hasInternet;
        private static Page currentPage;
        private static Timer timer;
        private static bool noInterShow;

        public App()
        {
            InitializeComponent();
            LocalizeService.SetLocalization();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public static RestService RestService
        {
            get
            {
                if(restService == null)
                {
                    restService = new RestService();
                }
                return restService;
            }
        }


        public static void StartCheckIfInternet(Label label, Page page)
        {
            labelScreen = label;
            label.Text = Localization.Resources.TrTextNoInternet;
            label.IsVisible = false;
            hasInternet = true;
            currentPage = page;
            if(timer == null)
            {
                timer = new Timer((e) =>
                {
                    CheckIfInternetOverTime();
                }, null, 10, (int)TimeSpan.FromSeconds(3).TotalMilliseconds);
            }

        }

        private static void CheckIfInternetOverTime()
        {
            var networkConnection = DependencyService.Get<INetworkConnection>();
            networkConnection.CheckNetworkConnection();
            if (!networkConnection.IsConnected)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (hasInternet)
                    {
                        if (!noInterShow)
                        {
                            hasInternet = false;
                            labelScreen.IsVisible = true;
                            await ShowDisplayAlert();
                        }
                    }
                });
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    hasInternet = true;
                    labelScreen.IsVisible = false;
                });
            }
        }

        public static async Task<bool> CheckIfInternet()
        {
            var networkConnection = DependencyService.Get<INetworkConnection>();
            networkConnection.CheckNetworkConnection();
            return networkConnection.IsConnected;    
        }

        public async static Task<bool> CheckIfInternetAlert()
        {
            var networkConnection = DependencyService.Get<INetworkConnection>();
            networkConnection.CheckNetworkConnection();
            if (!networkConnection.IsConnected)
            {
                if (!noInterShow)
                {

                    await ShowDisplayAlert();
                }
                return false;
            }
            return true;
        }

        private async static Task ShowDisplayAlert()
        {
            noInterShow = false;
            await currentPage.DisplayAlert("Internet", "Device has no internet, please reconnect", "Oke");
            noInterShow = false;
        }
    }
}
