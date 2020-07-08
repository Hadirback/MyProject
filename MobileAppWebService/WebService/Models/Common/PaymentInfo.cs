using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlotLogic.MobileAppWebService.Models.Common
{
    public class PaymentInfo
    {
        public String CreationDate { get; set; }
        public String CompletionDate { get; set; }
        public Int32? CommandType { get; set; }
        public Decimal? Amount { get; set; }
        public Decimal? ChangeAmount { get; set; }
        public Int32? PaymentStatus { get; set; }
        public String GmNumber { get; set; }
    }
}
