
using System.Collections.Generic;
using Slotlogic.MobileApp.Localization;
using Slotlogic.MobileApp.Models.Common.Main;

namespace Slotlogic.MobileApp.Models.InputData
{
    public class AuthenticationData
    {
        public string ClubID { get; private set; }
        public string Password { get; private set; }
        public CardInfo Card { get; private set; }

        public AuthenticationData() { }

        public AuthenticationData(string clubId, CardInfo card, string password)
        {
            ClubID = clubId;
            Card = card;
            Password = password;
        }
    }
}
