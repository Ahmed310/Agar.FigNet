using FigNet.Core;
using AgarIOCommon;
using AgarIOGame.Game;
using AgarIOCommon.DataModel;

namespace AgarIOGame.Messages.Handlers
{
    public class PlayerKilledHandler : IHandler
    {
        public ushort MsgId => (ushort)MessageId.PlayerKilled;

        public void HandleMessage(Message message, uint PeerId)
        {
            //var peer = FN.PeerCollection.GetPeerByID(PeerId);
            var zone = ServiceLocator.GetService<Zone>();

            var data = message.Payload as PlayerKilledData;
            uint duel1 = data.playerKilledId;
            uint duel2 = data.KilledById;
            FN.Logger.Error($"p1 {duel1} | {duel2} p2");
            zone.PlayerKilled(data.playerKilledId, data.KilledById);

            PlayerKilledData.Release(data);
        }
    }
}
