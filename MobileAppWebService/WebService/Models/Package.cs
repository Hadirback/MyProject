using SlotLogic.MobileAppWebService.Models.Enum;

namespace SlotLogic.MobileAppWebService.Models
{
    public class Package<T>
    {
        public Status Status { get; set; }
        public int Code { get; set; }        
        public string Type { get; set; }
        public T Data { get; set; }

        public Package(Status status, T data, int code = 0, TypeError type = default)
        {
            Status = status;
            Data = data;
            Code = code;
            Type = type.ToString();
        }
    }
}
