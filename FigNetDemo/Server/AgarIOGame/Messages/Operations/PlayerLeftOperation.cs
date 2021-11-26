using FigNet.Core;
using AgarIOCommon;
using AgarIOCommon.DataModel;

namespace AgarIOGame.Messages.Operations
{
    public class PlayerLeftOperation
    {
        private static readonly ushort mId = (ushort)MessageId.PlayerLeft;
        public static Message Get(uint Id)
        {
            PlayerLeftData payload = PlayerLeftData.Acquire();

            payload.Id = Id;

            var msg = Message.Acquire();
            msg.Id = mId;
            msg.Payload = payload;

            msg.OnMessageSent = () => {

                Message.Release(msg);
                PlayerLeftData.Release(payload);
            };

            return msg;
        }
    }
}
