using System;
using FigNet.Core;

namespace AgarIOCommon.DataModel
{
    public class PlayerLeftData
    {
        private static Pool<PlayerLeftData> Pool = new Pool<PlayerLeftData>(() => new PlayerLeftData(), (op) => op.Reset(), 4);
        public uint Id;


        public void Reset()
        {
            Id = 0;
        }

        public static PlayerLeftData Acquire()
        {
            return Pool.Acquire();
        }

        public static void Release(PlayerLeftData obj)
        {
            Pool.Release(obj);
        }

        public static object Deserialize(ArraySegment<byte> buffer)
        {
            var data = BitBufferPool.GetInstance();
            data.FromArray(buffer);

            var payload = Acquire();      // release it after use [see PingCountHandler.cs ln 21]

            payload.Id = data.ReadUInt();

            return payload;
        }

        public static ArraySegment<byte> Serialize(object pingOperation)
        {
            var op = pingOperation as PlayerLeftData;
            var data = BitBufferPool.GetInstance();
            data.Clear();

            data.AddUInt(op.Id);

            return data.ToArray();
        }
    }
}
