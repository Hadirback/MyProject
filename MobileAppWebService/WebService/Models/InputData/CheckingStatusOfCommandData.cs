using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlotLogic.MobileAppWebService.Models.InputData
{
    public class CheckingStatusOfCommandData
    {
        public Int32? GameMachineId { get; set; }
        public Int32? CardId { get; set; }
        public String ClubId { get; set; }
        public Int32? CommandType { get; set; }
    }
}
