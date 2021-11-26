using System;
using FigNet.Core;

namespace AgarIOCommon.DataModel
{
    public class PlayerKilledData
    {
        private static Pool<PlayerKilledData> Pool = new Pool<PlayerKilledData>(() => new PlayerKilledData(), (op) => op.Reset(), 4);
        public uint playerKilledId;
        public uint KilledById;
        public uint Score;
        public void Reset()
        {
            playerKilledId = 0;
            KilledById = 0;
            Score = 0;
        }

        public static PlayerKilledData Acquire()
        {
            return Pool.Acquire();
        }

        public static void Release(PlayerKilledData obj)
        {
            Pool.Release(obj);
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
