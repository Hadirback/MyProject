using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlotLogic.MobileAppWebService.Models.Common
{
    public class GmBalanceInfo
    {
        public Int32? GmId { get; set; }
        public String GmNumber { get; set; }
        public Decimal? GmBalance { get; set; } 
        public Decimal? CardBalance { get; set; }
    }
}
