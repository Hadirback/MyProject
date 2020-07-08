using Slotlogic.MobileApp.Models.Common.PBI;

namespace Slotlogic.MobileApp.Models.OutputData
{
    public class ModelResourcesDataPlayer
    {
        public Error ErrorData { get; set; }
        public ResourcesDataPlayer PBIData { get; set; }

        public ModelResourcesDataPlayer() { }
        public ModelResourcesDataPlayer(Error error)
        {
            ErrorData = error;
            PBIData = new ResourcesDataPlayer();
        }

        public ModelResourcesDataPlayer(ResourcesDataPlayer data)
        {
            ErrorData = new Error();
            PBIData = data;
        }
    }
}
