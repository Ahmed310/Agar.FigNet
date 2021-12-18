using System;
using FigNet.Core;

namespace AgarIOCommon.DataModel
{
    public class PlayerLeftData
    {
        public uint Id;
        public void Reset()
        {
            Id = 0;
        }

        public static PlayerLeftData Acquire()
        {
            return new PlayerLeftData();
        }

        public static object Deserialize(ArraySegment<byte> buffer)
        {
            var data = BitBufferPool.GetInstance();
            data.FromArray(buffer);

            var payload = Acquire();      

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
