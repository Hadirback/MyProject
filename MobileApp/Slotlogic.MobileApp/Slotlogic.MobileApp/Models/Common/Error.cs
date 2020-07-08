using System;
using Slotlogic.MobileApp.Models.Enum;

namespace Slotlogic.MobileApp.Models
{
    public class Error
    {
        public int? Code { get; set; }
        public string Description { get; set; }
        public TypeError TypeError { get; set; }
    }
}
