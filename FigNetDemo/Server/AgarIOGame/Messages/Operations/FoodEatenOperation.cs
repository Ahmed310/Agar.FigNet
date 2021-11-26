using FigNet.Core;
using AgarIOCommon;
using AgarIOCommon.DataModel;

namespace AgarIOGame.Messages.Operations
{
    public class FoodEatenOperation
    {
        private static readonly ushort mId = (ushort)MessageId.FoodEaten;
        public static Message Get(uint foodId, uint playerId)
        {
            FoodEatenData payload = FoodEatenData.Acquire();
            
            payload.FoodId = foodId;
            payload.PlayerId = playerId;

            var msg = Message.Acquire();
            msg.Id = mId;
            msg.Payload = payload;

            msg.OnMessageSent = () => {

                Message.Release(msg);
                FoodEatenData.Release(payload);
            };

            return msg;
        }
    }
}
