using System;
using FigNet.Core;

namespace AgarIOGame.Game
{
    public class ConnectionManager : IServerSocketListener
    {
        public void OnError(Exception e, string message)
        {
            FN.Logger.Exception(e, message);
        }

        public void OnInitilize(IServerSocket serverSocket)
        {
            FN.Logger.Info($"ONInit- ConnManager from Zone Module");
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
            FN.Logger.Info($"ZONE Player Connected: {peer.Id}");
        }

        public void OnPeerDisconnected(IPeer peer)
        {
            FN.Logger.Info($"ZONE Player Disconnected: {peer.Id}");
            
            var zone = ServiceLocator.GetService<Zone>();
            zone.RemovePlayer(peer);
        }

        public void OnProcessPayloadException(ExceptionType type, ushort messageId, Exception e)
        {
            FN.Logger.Exception(e, $"{type}|{messageId}");
        }
    }
}
