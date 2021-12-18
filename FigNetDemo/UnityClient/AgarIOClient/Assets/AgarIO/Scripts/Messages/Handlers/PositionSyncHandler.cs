using FigNet.Core;
using AgarIOCommon;
using AgarIOCommon.DataModel;

public class PositionSyncHandler : IHandler
{
    public ushort MsgId => (ushort)MessageId.PositionSync;

    public void HandleMessage(Message message, uint PeerId)
    {
        var data = message.Payload as PositionSyncData;
        // here add logic

        var player = NetworkEntitiesContainer.GetPlayerById(data.Id);
        if (player != null)
        {
            player.UpdatePosition(data);
        }

    }
}