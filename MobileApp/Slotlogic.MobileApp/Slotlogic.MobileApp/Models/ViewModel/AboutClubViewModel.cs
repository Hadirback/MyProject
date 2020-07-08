
namespace Slotlogic.MobileApp.Models.ViewModel
{
    public class AboutClubViewModel : BaseViewModel
    {
        private string _aboutClub;
        public string AboutClub
        {
            get { return _aboutClub; }
            set
            {
                _aboutClub = value;
                OnPropertyChanged();
            }
        }

        public AboutClubViewModel() {}
        public AboutClubViewModel(string data)
        {
            AboutClub = data.Replace("\r", "\n");
        }
    }
}
