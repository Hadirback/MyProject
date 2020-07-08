using System;

namespace SlotLogic.MobileAppWebService.Models.Common
{
    public class PlayerBalancesInfo
    {
        public Boolean CashbackActive { get; set; }
        public String PlayerFirstName { get; set; }
        public String PlayerSurname { get; set; }
        public String PlayerStatus { get; set; }
        public Int32? PtsBalance { get; set; }
        public Decimal? CashbackAmount { get; set; }
        public Decimal? Balance { get; set; }
    }
}
