using Slotlogic.MobileApp.Models.Common;
using System.Collections.Generic;

namespace Slotlogic.MobileApp.Models.OutputData
{
    public class ModelDraws
    {
        public Error ErrorData { get; set; }
        public List<DrawsInfo> DrawsList { get; set; }

        public ModelDraws() { }
        public ModelDraws(Error error)
        {
            DrawsList = new List<DrawsInfo>();
            ErrorData = error;
        }

        public ModelDraws(List<DrawsInfo> listData)
        {
            DrawsList = listData;
            ErrorData = new Error();
        }
    }
}
