using System;
using FigNet.Core;

namespace AgarIOCommon.DataModel
{
    public class PlayerKilledData
    {
        public uint playerKilledId;
        public uint KilledById;
        public uint Score;

        public static PlayerKilledData Acquire()
        {
            return new PlayerKilledData();
        }
      
        public static object Deserialize(ArraySegment<byte> buffer)
        {
            var data = BitBufferPool.GetInstance();
            data.FromArray(buffer);

            var payload = Acquire();      

            payload.playerKilledId = data.ReadUInt();
            payload.KilledById = data.ReadUInt();
            payload.Score = data.ReadUInt();

            return payload;
        }

        public static ArraySegment<byte> Serialize(object pingOperation)
        {
            var op = pingOperation as PlayerKilledData;
            var data = BitBufferPool.GetInstance();
            data.Clear();

            data.AddUInt(op.playerKilledId);
            data.AddUInt(op.KilledById);
            data.AddUInt(op.Score);

            return data.ToArray();
        }
    }
}
