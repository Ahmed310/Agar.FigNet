using FigNet.Core;
using AgarIOCommon;
using AgarIOCommon.DataModel;

public class PlayerLeftHandler : IHandler
{
    public ushort MsgId => (ushort)MessageId.PlayerLeft;

    public void HandleMessage(Message message, uint PeerId)
    {
        var data = message.Payload as PlayerLeftData;
        // here add logic
        NetworkEntitiesContainer.RemovePlayer(data.Id);
        FN.Logger.Info($"On Player Left");

    }
}