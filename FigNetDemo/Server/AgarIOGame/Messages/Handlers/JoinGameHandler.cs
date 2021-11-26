using FigNet.Core;
using AgarIOCommon;
using AgarIOGame.Game;
using AgarIOCommon.DataModel;
using AgarIOGame.Messages.Operations;

namespace AgarIOGame.Messages.Handlers
{
    public class JoinGameHandler : IHandler
    {
        public ushort MsgId => (ushort)MessageId.JoinGame;

        public void HandleMessage(Message message, uint PeerId)
        {
            var peer = FN.PeerCollection.GetPeerByID(PeerId);
            var zone = ServiceLocator.GetService<Zone>();

            var joinGameData = message.Payload as JoinGameData;

            if (peer == null)
            {
                FN.Logger.Error($"Woooooooo it was causing the App Crash {PeerId}");
                return;
            }
            FN.Logger.Error($"player Joined {joinGameData.Name} | {joinGameData.Color} peer {peer.Id} provider {peer.Provider}");
            
            FN.Server.SendMessage(peer, JoinGameOperation.Get(joinGameData.Name, joinGameData.Color, joinGameData.Position), DeliveryMethod.Reliable, 0);
            
            zone.AddPlayer(peer, joinGameData.Name, joinGameData.Color, joinGameData.Position);


            JoinGameData.Release(joinGameData);
        }
    }
}
