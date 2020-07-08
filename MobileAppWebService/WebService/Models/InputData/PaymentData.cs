using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlotLogic.MobileAppWebService.Models.InputData
{
    public class PaymentData
    {
        public AuthenticationData AuthenticationData { get; set; }

        public Int32? GameMachineId { get; set; }
        public Decimal? Amount { get; set; }
    }
}
 