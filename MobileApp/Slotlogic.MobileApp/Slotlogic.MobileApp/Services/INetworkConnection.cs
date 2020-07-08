using System;
using System.Collections.Generic;
using System.Text;

namespace Slotlogic.MobileApp.Services
{
    public interface INetworkConnection
    {
        bool IsConnected { get; }
        void CheckNetworkConnection();
    }
}
