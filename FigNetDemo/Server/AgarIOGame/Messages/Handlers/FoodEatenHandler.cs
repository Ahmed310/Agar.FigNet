using FigNet.Core;
using AgarIOCommon;
using AgarIOGame.Game;
using AgarIOCommon.DataModel;

namespace AgarIOGame.Messages.Handlers
{
    public class FoodEatenHandler : IHandler
    {
        public ushort MsgId => (ushort)MessageId.FoodEaten;

        public void HandleMessage(Message message, uint PeerId)
        {
            var peer = FN.PeerCollection.GetPeerByID(PeerId);
            var zone = ServiceLocator.GetService<Zone>();

            var data = message.Payload as FoodEatenData;

            zone.FoodEaten(data.FoodId, data.PlayerId);
        }
    }
}
