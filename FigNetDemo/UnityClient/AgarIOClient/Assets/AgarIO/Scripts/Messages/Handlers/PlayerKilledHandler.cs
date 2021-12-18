using FigNet.Core;
using AgarIOCommon;
using AgarIOCommon.DataModel;

public class PlayerKilledHandler : IHandler
{
    public ushort MsgId => (ushort)MessageId.PlayerKilled;

    public void HandleMessage(Message message, uint PeerId)
    {
        var data = message.Payload as PlayerKilledData;
        // here add logic
        uint killer = data.KilledById;
        uint killed = data.playerKilledId;


        var _kView = NetworkEntitiesContainer.GetPlayerById(killer);
        _kView.EatPlayer(data.Score);

        var killedView = NetworkEntitiesContainer.GetPlayerById(killed);
        if (killedView != null && killedView.NetworkPlayer.IsMine)
        {
            GameManager.GetInstance().OnGameEnded();
        }
        NetworkEntitiesContainer.RemovePlayer(killed);

        FN.Logger.Info($"On Player Killed");

    }
}