using System;
using FigNet.Core;

namespace AgarIOServer
{
    public class ConnectionManager : IServerSocketListener
    {
        public void OnError(Exception e, string message)
        {
            FN.Logger.Exception(e, message);
        }

        public void OnInitilize(IServerSocket serverSocket)
        {

        }

        public void OnNetworkReceive(Message message, uint sender)
        {
            bool handled = FN.HandlerCollection.HandleMessage(message, sender);
            if (!handled)
            {
                // no handler is registered against coming msg, mannually handle it here
                FN.Logger.Info($"no handler is registered against coming msgId: {message.Id}");
            }
        }

        public void OnNetworkSent(Message message, DeliveryMethod method, byte channelId = 0)
        {
        }

        public void OnPeerConnected(IPeer peer)
        {
            FN.Logger.Info($"Player Connected: {peer.Id}");
        }

        public void OnPeerDisconnected(IPeer peer)
        {
            FN.Logger.Info($"Player Disconnected: {peer.Id}");

        }
    }
}
