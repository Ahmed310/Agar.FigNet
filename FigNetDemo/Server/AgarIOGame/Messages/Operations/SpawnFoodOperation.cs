using FigNet.Core;
using AgarIOCommon;
using System.Numerics;
using AgarIOCommon.DataModel;

namespace AgarIOGame.Messages.Operations
{
    public class SpawnFoodOperation
    {
        private static readonly ushort mId = (ushort)MessageId.SpawnFood;
        public static Message Get(uint id, Vector2 position, byte colorId)
        {
            SpawnFoodData payload = SpawnFoodData.Acquire();
            
            payload.Id = id;
            payload.Position = position;
            payload.ColorId = colorId;

            var msg = Message.Acquire();
            msg.Id = mId;
            msg.Payload = payload;

            msg.OnMessageSent = () => {

                Message.Release(msg);
                SpawnFoodData.Release(payload);
            };
            return msg;
        }
    }
}
