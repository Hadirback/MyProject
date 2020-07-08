using Slotlogic.MobileApp.Localization;
using Slotlogic.MobileApp.Models.InputData;
using Slotlogic.MobileApp.Models.OutputData;
using Slotlogic.MobileApp.Models.View;
using Slotlogic.MobileApp.Models.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Slotlogic.MobileApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayerInfoDetailPage : ContentPage
    { 
        public PlayerInfoDetailPage() {}

        public PlayerInfoDetailPage(PlayerInfoViewModel data)
        {
            InitializeComponent();
            if (!data.CashbackActive)
            {
                LabelDetailsCashback.IsVisible = false;
                LabelDetailsCashbackValue.IsVisible = false;
            }
            this.BindingContext = data;       
            App.StartCheckIfInternet(LabelNoInternet, this);
            

            refreshLayout.SetBinding<PlayerInfoViewModel>(PullToRefreshLayout.IsRefreshingProperty, vm => vm.IsBusy, BindingMode.OneWay);
            refreshLayout.SetBinding<PlayerInfoViewModel>(PullToRefreshLayout.RefreshCommandProperty, vm => vm.RefreshCommand);
        }
    }
}