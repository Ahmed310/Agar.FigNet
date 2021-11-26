using FigNet.Core;
using AgarIOCommon;
using AgarIOCommon.DataModel;

public class PlayerKilledOperation
{
    private static readonly ushort mId = (ushort)MessageId.PlayerKilled;
    public static Message Get(uint playerKilledId, uint killedById)
    {
        PlayerKilledData payload = PlayerKilledData.Acquire();

        payload.playerKilledId = playerKilledId;
        payload.KilledById = killedById;
        payload.Score = 0;

        var msg = Message.Acquire();
        msg.Id = mId;
        msg.Payload = payload;

        msg.OnMessageSent = () => {

            Message.Release(msg);
            PlayerKilledData.Release(payload);
        };

        return msg;
    }
}