using Slotlogic.MobileApp.Models.Enum;

namespace Slotlogic.MobileApp.Models.Common
{
    public class Package<T>
    {
        public Statuses Status { get; set; }
        public T Data { get; set; }
    }
}
