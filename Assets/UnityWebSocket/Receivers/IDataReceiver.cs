using System;

namespace Unity3dAzure.WebSockets
{
    public interface IDataReceiver
    {
        void OnReceivedText(object sender, TextEventArgs args);
        void OnReceivedBinary(object sender, BinaryEventArgs args);
    }
}
