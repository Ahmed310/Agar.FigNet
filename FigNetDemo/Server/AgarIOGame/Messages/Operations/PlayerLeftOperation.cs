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

            var msg = new Message();
            msg.Id = mId;
            msg.Payload = payload;

            return msg;
        }
    }
}
