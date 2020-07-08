using System;

using SlotLogic.MobileAppWebService.Models.Common;

namespace SlotLogic.MobileAppWebService.Models.InputData
{
    public class AuthenticationData
    {
        public String ClubID { get; set; }
        public CardInfo Card { get; set; }
        public String Password { get; set; }
    }
}
