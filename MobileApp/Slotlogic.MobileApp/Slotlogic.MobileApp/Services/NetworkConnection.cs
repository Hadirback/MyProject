using Plugin.Connectivity;
using Slotlogic.MobileApp.Services;

[assembly: Xamarin.Forms.Dependency(typeof(NetworkConnection))]
namespace Slotlogic.MobileApp.Services
{
    public class NetworkConnection : INetworkConnection
    {
        public bool IsConnected { get; set; }

        public void CheckNetworkConnection()
        {
            if(CrossConnectivity.Current.IsConnected)
                IsConnected = true;
            else
                IsConnected = false;
        }


    }
}
