using Slotlogic.MobileApp.Models.Common.PBI;

namespace Slotlogic.MobileApp.Models.OutputData
{
    public class ModelPlayerBalances
    {
        public Error ErrorData { get; set; }
        public PlayerBalancesInfo PBIData { get; set; }

        public ModelPlayerBalances() { }
        public ModelPlayerBalances(Error error)
        {
            ErrorData = error;
            PBIData = new PlayerBalancesInfo();
        }

        public ModelPlayerBalances(PlayerBalancesInfo data)
        {
            ErrorData = new Error();
            PBIData = data;
        }
    }
}
