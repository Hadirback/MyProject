using System;
using Xamarin.Forms;
using Slotlogic.MobileApp.Models.InputData;
using Slotlogic.MobileApp.Models.OutputData;
using Slotlogic.MobileApp.Pages;
using Slotlogic.MobileApp.Models.Common;
using Slotlogic.MobileApp.Services;
using Slotlogic.MobileApp.Models.Enum;
using Slotlogic.MobileApp.Models.ViewModel;
using System.Diagnostics;
using Translate = Slotlogic.MobileApp.Localization.Resources;
using Plugin.Connectivity;
using Slotlogic.MobileApp.Models.Common.Main;

namespace Slotlogic.MobileApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {  
            InitializeComponent();
            Init(); 
        }

        void Init()
        {
            // If there is no internet connection, show the warning box.
            App.StartCheckIfInternet(LabelNoInternet, this);
            ActivitySpinner.IsVisible = false;   
            this.BindingContext = new MainViewModel();

            EntryClubID.Completed += (s, e) => EntryCardNumber.Focus();
            EntryCardNumber.Completed += (s, e) => EntryPass.Focus();
            EntryPass.Completed += (s, e) => MainButton_Clicked(s, e);
        }

        private async void MainButton_Clicked(object sender, EventArgs e)
        {
            if(!CrossConnectivity.Current.IsConnected)
                await Service.ErrorDisplay(Translate.TrMainPageEnterClubID, this);
            else
            if (string.IsNullOrWhiteSpace(EntryClubID.Text))
                await Service.ErrorDisplay(Translate.TrMainPageEnterClubID, this);
            else
            if (string.IsNullOrWhiteSpace(EntryCardNumber.Text))
                await Service.ErrorDisplay($"{Translate.TrMainPageEnterCardNumber}", this);
            else
            if (!Service.IsCorrectCardNumber(EntryCardNumber.Text.Trim()))
                await Service.ErrorDisplay($"{Translate.TrMainPageEnterCardValid}", this);
            else
            if (string.IsNullOrWhiteSpace(EntryPass.Text))
                await Service.ErrorDisplay($"{Translate.TrMainPageEnterPassword}", this);
            else
            if(!Service.IsConnectedToInternet(Settings.IpAddress))
                await Service.ErrorDisplay($"{Translate.TrMainPageErrorServer}", this);
            else
            {
                CardInfo card = Service.SplitCardNumber(EntryCardNumber.Text.Trim());
                AuthenticationData authorisationData = new AuthenticationData(EntryClubID.Text.ToLower().Trim(), card, EntryPass.Text);

                Package<ModelPlayerBalances> jsonOutput = null;

                this.ActivitySpinner.IsVisible = true;
                try
                {
                    // Get the PlayerBalances info
                    jsonOutput = await App.RestService.PostResponse<Package<ModelPlayerBalances>>(authorisationData, Settings.LogInSystem);

                    if (jsonOutput == null)
                    {
                        Service.WriteToLog($"ModelPlayerBalances jsonOutput == null on MainPage page.", page: this);
                        this.ActivitySpinner.IsVisible = false;
                        return;
                    }
                }
                catch (Exception exc)
                {
                    Service.WriteToLog($"Failed to get ModelPlayerBalances data on MainPage page.", exc, this);
                    this.ActivitySpinner.IsVisible = false;
                    return;
                }

                if (jsonOutput.Status == Statuses.Succeed)
                {
                    // Get the ClubName by ID
                    Package<ModelClub> jsonOutputClub = null;
                    try
                    {
                        jsonOutputClub = await App.RestService.GetResponse<Package<ModelClub>>(Settings.AboutClubInfo, authorisationData.ClubID);
                    }
                    catch (Exception exc)
                    {
                        Service.WriteToLog($"Failed to get ModelClub data on MainPage page.", exc);
                    }

                    // If it was not possible to get the name of the club, then we get its ID
                    string clubName;
                    if (jsonOutputClub != null)
                        clubName = jsonOutput.Status == Statuses.Succeed ? jsonOutputClub.Data.ClubName : authorisationData.ClubID;
                    else clubName = authorisationData.ClubID;

                    try
                    {
                        BaseMDPage masterDetailPage = new BaseMDPage(jsonOutput.Data.PBIData, authorisationData, clubName);
                        Application.Current.MainPage = masterDetailPage;
                    }
                    catch(Exception exc)
                    {
                        Service.WriteToLog($"Could not create BaseMDPage page.", exc, this);
                    }
                    this.ActivitySpinner.IsVisible = false;
                }
                else
                {
                    try
                    {
                        if (jsonOutput.Data.ErrorData.TypeError == TypeError.User)
                            await Service.ErrorDisplay(CardInfo.CodesError[(int)jsonOutput.Data.ErrorData.Code], this);
                        else
                        {
                            Debug.WriteLine($"Type Error {jsonOutput.Data.ErrorData.TypeError}, Description {jsonOutput.Data.ErrorData.Description}, Code Error {jsonOutput.Data.ErrorData.Code}");
                        }
                        this.ActivitySpinner.IsVisible = false;
                    }
                    catch(Exception exc)
                    {
                        Service.WriteToLog($"Don't get Error message on MainPage page.", exc, this);
                        this.ActivitySpinner.IsVisible = false;
                    }
                }
            }
        }

        private void ImageMainButton_Clicked(object sender, EventArgs e)
        {
            MainSettingsPage page = new MainSettingsPage();
            Application.Current.MainPage = page;     
        }
    }
}
