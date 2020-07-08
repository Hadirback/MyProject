using Slotlogic.MobileApp.Models.Common;
using Slotlogic.MobileApp.Models.Enum;
using Slotlogic.MobileApp.Models.InputData;
using Slotlogic.MobileApp.Models.OutputData;
using Slotlogic.MobileApp.Models.ViewModel;
using Slotlogic.MobileApp.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Slotlogic.MobileApp.Models.Common.PBI;

namespace Slotlogic.MobileApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BaseMDPage : MasterDetailPage
    {
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();
        internal static AuthenticationData session { get; set; }

        public BaseMDPage() {}

        public BaseMDPage(PlayerBalancesInfo pbi, AuthenticationData authenticationData, string clubName)
        {
            InitializeComponent();
            this.BindingContext = new PlayerInfoViewModel(pbi, authenticationData, this);
            session = authenticationData;

            Detail = new NavigationPage(new PlayerInfoDetailPage((PlayerInfoViewModel)BindingContext));
            Master = new MenuPage(clubName);

            MasterBehavior = MasterBehavior.Popover;
            MenuPages.Add((int)MenuItemType.Profile, (NavigationPage)Detail);
        }

        public async Task NavigateFromMenu(int id)
        {
            // If the page opens for the first time, add it to the list MenuPages and load data from the database.
            if (!MenuPages.ContainsKey(id))
            {
                switch (id)
                {
                    case (int)MenuItemType.Profile:
                        {
                            // We do not load the Profile page from the database because the data was loaded earlier.
                            MenuPages.Add(id, new NavigationPage(new PlayerInfoDetailPage((PlayerInfoViewModel)BindingContext)));
                            break;
                        }
                    case (int)MenuItemType.Draws:
                        {
                            try
                            {
                                Package<ModelDraws> jsonOutput = await App.RestService.PostResponse<Package<ModelDraws>>(session, Settings.DrawsInfo);
                                if (jsonOutput == null)
                                {
                                    Service.WriteToLog($"Failed to get Draws data on BaseMDPage page. (jsonOutput == null)", page: this);
                                    return;
                                }

                                if (jsonOutput.Status == Statuses.Succeed)
                                {
                                    MenuPages.Add(id, new NavigationPage(new DrawsPage(jsonOutput.Data.DrawsList)));
                                    break;
                                }
                                else
                                {
                                    Service.WriteToLog($"Failed to get Draws data on BaseMDPage page", page: this);
                                    return;
                                }
                            }
                            catch(Exception exc)
                            {
                                Service.WriteToLog($"Failed to get Draws data on BaseMDPage page", exc, this);
                                return;
                            }
                        }
                    case (int)MenuItemType.AboutClub:
                        {
                            try
                            {
                                Package<ModelClub> jsonOutput = await App.RestService.GetResponse<Package<ModelClub>>(Settings.AboutClubInfo, session.ClubID);
                                if (jsonOutput == null)
                                {
                                    Service.WriteToLog($"Failed to get AboutClub data on BaseMDPage page. (jsonOutput == null)", page: this);
                                    return;
                                }

                                if (jsonOutput.Status == Statuses.Succeed)
                                {
                                    MenuPages.Add(id, new NavigationPage(new AboutClubPage(jsonOutput.Data.AboutClub)));
                                    break;
                                }
                                else
                                {
                                    Service.WriteToLog($"An error occurred while retrieving data about the club on BaseMDPage page.", page: this);
                                    return;
                                }
                            }
                            catch (Exception exc)
                            {
                                Service.WriteToLog($"Failed to get AboutClub data on BaseMDPage page", exc);
                                return;
                            }
                        }
                    case (int)MenuItemType.Exit:
                        {
                            // Exit to the main page.
                            Application.Current.MainPage = new MainPage();
                            return;
                        }
                }
            }

            var newPage = MenuPages[id];

            if (newPage != null && Detail != newPage)
            {
                Detail = newPage;

                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(10);
            }
        }
    }
}