using FigNet.Core;
using AgarIOCommon;
using AgarIOCommon.DataModel;

public class SpawnLocalPlayerHandler : IHandler
{
    public ushort MsgId => (ushort)MessageId.SpawnLocalPlayer;

    public void HandleMessage(Message message, uint PeerId)
    {
        var data = message.Payload as SpawnLocalPlayerData;
        // here add logic
        var netPlayer = new NetworkPlayer() 
        {
            Id = data.Id,
            IsMine = true,
            Position = data.Position,
            Name = GameManager.MyName,
            Color = new System.Numerics.Vector3(GameManager.MyColor.r, GameManager.MyColor.g, GameManager.MyColor.b)
        };
        NetworkEntitiesContainer.AddNetworkPlayer(data.Id, netPlayer);
        GameManager.MyID = data.Id;
        FN.Logger.Info($"On spawn local player...{data.Id}");

        GameManager.GetInstance().UpdateScore(0);
    }
}