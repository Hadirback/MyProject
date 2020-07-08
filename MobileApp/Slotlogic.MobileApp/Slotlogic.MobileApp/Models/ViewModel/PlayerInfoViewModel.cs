using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Slotlogic.MobileApp.Models.Common.PBI;
using Slotlogic.MobileApp.Models.InputData;
using Xamarin.Forms;
using Slotlogic.MobileApp.Services;
using Slotlogic.MobileApp.Models.Common.Main;
using Slotlogic.MobileApp.Pages;

namespace Slotlogic.MobileApp.Models.ViewModel
{
    public class PlayerInfoViewModel : BaseViewModel
    {
        private Page _pbiPage = null;
        public Page PBIPage
        {
            get { return _pbiPage; } 
        }


        private bool _cashbackActive;
        public bool CashbackActive
        {
            get { return _cashbackActive; }
            set
            {
                _cashbackActive = value;
                OnPropertyChanged();
            }
        }

        private string _playerFirstName;
        public string PlayerFirstName
        {
            get { return _playerFirstName; }
            set
            {
                _playerFirstName = value;
                OnPropertyChanged();
            }
        }

        private string _playerSurname;
        public string PlayerSurname
        {
            get { return _playerSurname; }
            set
            {
                _playerSurname = value;
                OnPropertyChanged();
            }
        }

        private string _playerStatus;
        public string PlayerStatus
        {
            get { return _playerStatus; }
            set
            {
                _playerStatus = value;
                OnPropertyChanged();
            }
        }

        private int? _ptsBalance = 0;
        public int? PtsBalance
        {
            get { return _ptsBalance; }
            set
            {
                _ptsBalance = value;
                OnPropertyChanged();
            }
        }

        private decimal? _cashbackAmount = 0m;
        public decimal? CashbackAmount
        {
            get { return _cashbackAmount; }
            set
            {
                _cashbackAmount = value;
                OnPropertyChanged();
            }
        }

        private string _clubID;
        public string ClubID
        {
            get { return _clubID; }
            set
            {
                _clubID = value;
                OnPropertyChanged();
            }
        }

        private CardInfo _card;
        public CardInfo CardNumber
        {
            get { return _card; }
            set
            {
                _card = value;
                OnPropertyChanged();
            }
        }

        public string FullCardName
        {
            get
            {
                if (CardNumber == null)
                    return string.Empty;
                return $"{CardNumber.CardSeries}-{CardNumber.CardCompany}-{CardNumber.CardNumber.ToString("D6")}"; 
            }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        
        public string FullName
        {
            get
            {
                return $"{PlayerFirstName} {PlayerSurname}!";
            }
        }

        bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                if (isBusy == value)
                    return;

                isBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }

        bool isVisible = true;
        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                if (isVisible == value)
                    return;

                isVisible = value;
                OnPropertyChanged("IsVisible");
            }
        }

        ICommand refreshCommand;

        public ICommand RefreshCommand
        {
            get { return refreshCommand ?? (refreshCommand = new Command(async () => await ExecuteRefreshCommand())); }
        }

        async Task ExecuteRefreshCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            ResourcesDataPlayer dataPlayer = await Service.UpdateResourcesDataPlayer(PBIPage);
            if (dataPlayer != null)
            {
                if (dataPlayer.CashbackActive == false)
                    IsVisible = false;
                else
                    IsVisible = true;
                

                CashbackActive = dataPlayer.CashbackActive;
                CashbackAmount = dataPlayer.CashbackAmount;
                PtsBalance = dataPlayer.PtsBalance;
                PlayerStatus = dataPlayer.PlayerStatus;
            }

            IsBusy = false;
        }

        public PlayerInfoViewModel() {}
        public PlayerInfoViewModel(PlayerBalancesInfo playerBalancesInfo, AuthenticationData authenticationData, Page page)
        {
            CashbackActive = playerBalancesInfo.CashbackActive;
            PlayerFirstName = playerBalancesInfo.PlayerFirstName;
            PlayerSurname = playerBalancesInfo.PlayerSurname;
            PlayerStatus = playerBalancesInfo.PlayerStatus;
            PtsBalance = playerBalancesInfo.PtsBalance;
            CashbackAmount = playerBalancesInfo.CashbackAmount;
            ClubID = authenticationData.ClubID;
            CardNumber = authenticationData.Card;
            _password = authenticationData.Password;
            _pbiPage = page;
        }
    }
}
