using FigNet.Core;
using AgarIOCommon;
using System.Numerics;
using AgarIOCommon.DataModel;

namespace AgarIOGame.Messages.Operations
{
    public class SpawnRemotePlayerOperation
    {
        private static readonly ushort mId = (ushort)MessageId.SpawnRemotePlayer;
        public static Message Get(uint id, string name, Vector3 color, Vector2 position, uint rank , uint score)
        {
            SpawnRemotePlayerData payload = SpawnRemotePlayerData.Acquire();

            payload.Id = id;
            payload.Name = name;
            payload.Color = color;
            payload.Position = position;
            payload.Rank = rank;
            payload.Score = score;

            var msg = new Message();
            msg.Id = mId;
            msg.Payload = payload;

            return msg;
        }
    }
}
