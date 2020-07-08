using Slotlogic.MobileApp.Models.Enum;
using Slotlogic.MobileApp.Models.ItemsModel;
using Slotlogic.MobileApp.Models.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Slotlogic.MobileApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        BaseMDPage RootPage { get => Application.Current.MainPage as BaseMDPage; }

        public MenuPage()
        { }

        public MenuPage(string clubName)
        {
            InitializeComponent();
            this.BindingContext = new MenuPageViewModel() { ClubName = clubName };
        }

        private async void MenuItemsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MenuPageItem;
            if (item == null)
                return;

            var id = (int)(item.Id);
            await RootPage.NavigateFromMenu(id);
        }

        private void MenuItemsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            MenuPageItem item = e.Item as MenuPageItem;
            if (item == null)
                return;

            if(item.Id != MenuItemType.Exit)
                RootPage.IsPresented = false;
        }
    }
}