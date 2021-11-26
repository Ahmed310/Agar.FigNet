using FigNet.Core;
using AgarIOCommon;
using System.Numerics;
using AgarIOCommon.DataModel;

namespace AgarIOGame.Messages.Operations
{
    public class JoinGameOperation
    {
        private static readonly ushort mId = (ushort)MessageId.JoinGame;
        public static Message Get(string name, Vector3 color, Vector2 position)
        {
            JoinGameData payload = JoinGameData.Acquire();
            
            payload.Name = name;
            payload.Color = color;
            payload.Position = position;

            var msg = Message.Acquire();
            msg.Id = mId;
            msg.Payload = payload;

            msg.OnMessageSent = () => {

                Message.Release(msg);
                JoinGameData.Release(payload);
            };

            return msg;
        }
    }
}
