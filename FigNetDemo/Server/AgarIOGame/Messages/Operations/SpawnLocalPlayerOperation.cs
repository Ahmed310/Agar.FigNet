using FigNet.Core;
using AgarIOCommon;
using System.Numerics;
using AgarIOCommon.DataModel;

namespace AgarIOGame.Messages.Operations
{
    public class SpawnLocalPlayerOperation
    {
        private static readonly ushort mId = (ushort)MessageId.SpawnLocalPlayer;
        public static Message Get(uint Id, Vector2 position)
        {
            SpawnLocalPlayerData payload = SpawnLocalPlayerData.Acquire();

            payload.Id = Id;
            payload.Position = position;

            var msg = Message.Acquire();
            msg.Id = mId;
            msg.Payload = payload;

            msg.OnMessageSent = () => {

                Message.Release(msg);
                SpawnLocalPlayerData.Release(payload);
            };

            return msg;
        }
    }
}
