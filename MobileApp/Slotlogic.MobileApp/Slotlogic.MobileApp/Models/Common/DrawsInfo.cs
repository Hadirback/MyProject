using System;
using System.Collections.Generic;
using System.Text;
using Slotlogic.MobileApp.Localization;

namespace Slotlogic.MobileApp.Models.Common
{
    public class DrawsInfo
    {
        public string DrawName { get; set; }
        public DateTime? DrawStartDate { get; set; }
        public DateTime? DrawEndDate { get; set; }
        public DateTime? DrawFinalDate { get; set; }
        public DateTime? BonusesTill { get; set; }
        public int? BonusesForPeriod { get; set; }
        public int? BonusesForToday { get; set; }
        public int? Coupons { get; set; }

        public string PeriodValue
        {
            get
            {
                if (DrawStartDate.HasValue)
                    return $"{Resources.LabelCommonFrom} {DrawStartDate.Value.ToString("d")} {Resources.LabelCommonTo} {DrawEndDate.Value.ToString("d")}";
                else
                    return string.Empty;
            }
        }

        public string FinalDateValue
        {
            get
            {
                if (DrawFinalDate.HasValue)
                    return $"{DrawFinalDate.Value.ToString("d")}";
                else
                    return string.Empty;
            }
        }

        public string BonusesTillLabel
        {
            get
            {
                if (BonusesTill.HasValue)
                    return $"{Resources.LabelDrawBonusesTill} {BonusesTill.Value.ToString("d")}:";
                else
                    return string.Empty;
            }
        }
    }
}
