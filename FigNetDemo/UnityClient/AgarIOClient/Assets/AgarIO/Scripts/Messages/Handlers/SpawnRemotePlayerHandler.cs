using FigNet.Core;
using AgarIOCommon;
using AgarIOCommon.DataModel;

public class SpawnRemotePlayerHandler : IHandler
{
    public ushort MsgId => (ushort)MessageId.SpawnRemotePlayer;

    public void HandleMessage(Message message, uint PeerId)
    {
        var data = message.Payload as SpawnRemotePlayerData;
        // here add logic

        var netPlayer = new NetworkPlayer()
        {
            Id = data.Id,
            IsMine = false,
            Position = data.Position,
            Color = data.Color,
            Name = data.Name,
            Rank = data.Rank,
            Score = data.Score
        };
        NetworkEntitiesContainer.AddNetworkPlayer(data.Id, netPlayer);
        SpawnRemotePlayerData.Release(data);
        //FN.Logger.Info($"On spawn remote player...{data.Id}|{data.Name} - score {data.Score} - - rank {data.Rank} pos {netPlayer.Position.X}|{netPlayer.Position.Y}");
    }
}