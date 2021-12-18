using FigNet.Core;
using AgarIOCommon;
using AgarIOCommon.DataModel;
public class FoodEatenOperation
{
    private static readonly ushort mId = (ushort)MessageId.FoodEaten;
    public static Message Get(uint foodId, uint playerId)
    {
        FoodEatenData payload = FoodEatenData.Acquire();

        payload.FoodId = foodId;
        payload.PlayerId = playerId;

        var msg = new Message();
        msg.Id = mId;
        msg.Payload = payload;

        return msg;
    }
}