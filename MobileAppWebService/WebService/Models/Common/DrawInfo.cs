using System;

namespace SlotLogic.MobileAppWebService.Models.Common
{
    public class DrawInfo
    {
        public String DrawName { get; set; }
        public String DrawStartDate { get; set; }
        public String DrawEndDate { get; set; }
        public String DrawFinalDate { get; set; }
        public String BonusesTill { get; set; }
        public Int32? BonusesForPeriod { get; set; }
        public Int32? BonusesForToday { get; set; }
        public Int32? Coupons { get; set; }
    }
}
