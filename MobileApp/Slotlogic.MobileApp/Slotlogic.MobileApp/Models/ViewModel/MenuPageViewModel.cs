using Slotlogic.MobileApp.Localization;
using Slotlogic.MobileApp.Models.Enum;
using Slotlogic.MobileApp.Models.ItemsModel;
using System.Collections.ObjectModel;

namespace Slotlogic.MobileApp.Models.ViewModel
{
    public class MenuPageViewModel : BaseViewModel
    {
        private string _clubName;
        public string ClubName
        {
            get { return _clubName; }
            set
            {
                _clubName = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<MenuPageItem> MenuItems { get; set; }

        public MenuPageViewModel()
        {
            MenuItems = new ObservableCollection<MenuPageItem>(new[]
            {
                new MenuPageItem { Id = MenuItemType.Profile, Title = Resources.MenuItemProfile },
                new MenuPageItem { Id = MenuItemType.Draws, Title = Resources.MenuItemDraws },
                new MenuPageItem { Id = MenuItemType.AboutClub, Title = Resources.MenuItemAboutClub},
                new MenuPageItem { Id = MenuItemType.Exit, Title = Resources.MenuItemExit }
            });
        }      
    }
}