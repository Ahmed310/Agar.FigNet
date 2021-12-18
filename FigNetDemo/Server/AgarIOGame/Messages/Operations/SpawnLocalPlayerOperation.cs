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

            var msg = new Message();
            msg.Id = mId;
            msg.Payload = payload;
           
            return msg;
        }
    }
}
