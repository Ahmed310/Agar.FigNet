using FigNet.Core;
using AgarIOCommon;
using AgarIOCommon.DataModel;

public class PlayerRankChangedHandler : IHandler
{
    public ushort MsgId => (ushort)MessageId.PlayerRankChange;

    public void HandleMessage(Message message, uint PeerId)
    {
        var data = message.Payload as RankChangedData;
        // here add logic
        var player = NetworkEntitiesContainer.GetPlayerById(data.Id);
        if (player != null)
        {
            player.NetworkPlayer.Rank = data.Rank;
        }

        GameManager.GetInstance().UpdateLeaderBoard();

        RankChangedData.Release(data);
    }
}