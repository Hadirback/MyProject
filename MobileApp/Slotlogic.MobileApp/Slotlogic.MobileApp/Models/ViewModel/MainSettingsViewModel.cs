using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Slotlogic.MobileApp.Localization;
using Slotlogic.MobileApp.Models.InterfaceModel;

namespace Slotlogic.MobileApp.Models.ViewModel
{
    public class MainSettingsViewModel : BaseViewModel
    {
        int _languageSelectedIndex;
        public int LanguageSelectedIndex
        {
            get
            {
                return _languageSelectedIndex;
            }
            set
            {
                if (_languageSelectedIndex != value)
                {
                    _languageSelectedIndex = value;

                    // trigger some action to take such as updating other labels or fields
                    OnPropertyChanged(nameof(LanguageSelectedIndex));
                    SelectedLanguage = LanguageItems[_languageSelectedIndex].Language;
                }
            }
        }

        public List<LanguageItem> LanguageItems { get; set; }
        public string SelectedLanguage { get; set; }

        public MainSettingsViewModel()
        {
            LanguageItems = new List<LanguageItem>(new[]
            {
                new LanguageItem { Id = 0, Language = Resources.PickerLanguageEnglish, Code="en-GB" },
                new LanguageItem { Id = 1, Language = Resources.PickerLanguageRussian, Code="ru-RU" }
            });
        }
    }
}
