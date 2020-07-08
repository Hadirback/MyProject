using Slotlogic.MobileApp.Models.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Slotlogic.MobileApp.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Diagnostics;
using System.Linq;

namespace Slotlogic.MobileApp.Models.ViewModel
{
    public class DrawsViewModel : BaseViewModel
    {
        private Page _drawPage = null;
        public Page DrawPage
        {
            get { return _drawPage; }
        }

        private bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;

                    await RefreshData();

                    IsRefreshing = false;
                });
            }
        }

        async Task RefreshData()
        {
            try
            {
                List<DrawsInfo> draws = await Service.UpdateDrawsList(DrawPage);
                if (draws.Any())
                {
                    DrawsList.Clear();
                    foreach (var row in draws)
                        DrawsList.Add(row);
                }
             }
            catch(Exception exc)
            {
                Debug.WriteLine(exc.Message);
            }
        }

        public DrawsViewModel() { }

        public ObservableCollection<DrawsInfo> DrawsList { get; set; }

        public DrawsViewModel(List<DrawsInfo> data, Page drawPage)
        {
            DrawsList = new ObservableCollection<DrawsInfo>(data);
            _drawPage = drawPage;
        }

    }
}
