using FigNet.Core;
using AgarIOCommon;
using System.Numerics;
using AgarIOCommon.DataModel;
public class PositionSyncOperation
{
    private static readonly ushort mId = (ushort)MessageId.PositionSync;
    public static Message Get(uint id, Vector2 position)
    {
        PositionSyncData payload = PositionSyncData.Acquire();

        payload.Id = id;
        payload.Position = position;

        var msg = new Message();
        msg.Id = mId;
        msg.Payload = payload;
       

        return msg;
    }
}