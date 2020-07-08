using System.Collections.Generic;
using Slotlogic.MobileApp.Localization;

namespace Slotlogic.MobileApp.Models.Common.Main
{
    public class CardInfo
    {
        public string CardSeries { get; set; }
        public string CardCompany { get; set; }
        public int CardNumber { get; set; }

        public CardInfo(string series, string company, int number)
        {
            CardSeries = series;
            CardCompany = company;
            CardNumber = number;
        }

        public static readonly Dictionary<int, string> CodesError = new Dictionary<int, string>
        {
            { 1001, Resources.TrCodeErrorUser1001 },
            { 1002, Resources.TrCodeErrorUser1002 },
            { 1003, Resources.TrCodeErrorUser1003 },
            { 1004, Resources.TrCodeErrorUser1004 },
            { 1005, Resources.TrCodeErrorUser1005 },
            { 1006, Resources.TrCodeErrorUser1006 },
            { 1007, Resources.TrCodeErrorUser1007 }
        };
    }
}
