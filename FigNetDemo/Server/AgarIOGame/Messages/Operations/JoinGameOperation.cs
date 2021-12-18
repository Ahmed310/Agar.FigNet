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

            var msg = new Message();
            msg.Id = mId;
            msg.Payload = payload;

            return msg;
        }
    }
}
