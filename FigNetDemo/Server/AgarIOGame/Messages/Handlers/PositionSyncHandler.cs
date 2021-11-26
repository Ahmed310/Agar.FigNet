using FigNet.Core;
using AgarIOCommon;
using AgarIOGame.Game;
using AgarIOCommon.DataModel;

namespace AgarIOGame.Messages.Handlers
{
    public class PositionSyncHandler : IHandler
    {
        public ushort MsgId => (ushort)MessageId.PositionSync;

        public void HandleMessage(Message message, uint PeerId)
        {
            var peer = FN.PeerCollection.GetPeerByID(PeerId);
            var zone = ServiceLocator.GetService<Zone>();

            var data = message.Payload as PositionSyncData;

            zone.UpdatePosition(PeerId, data);
            PositionSyncData.Release(data);
        }
    }
}
