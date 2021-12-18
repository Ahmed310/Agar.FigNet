using System;
using FigNet.Core;

namespace AgarIOCommon.DataModel
{
    public class RankChangedData
    {
        public uint Id;
        public uint Rank;

        public void Reset() 
        {
            Id = 0;
            Rank = 0;
        }

        public static RankChangedData Acquire()
        {
            return new RankChangedData();
        }

       
        public static object Deserialize(ArraySegment<byte> buffer)
        {
            var data = BitBufferPool.GetInstance();
            data.FromArray(buffer);

            var payload = Acquire();

            payload.Id = data.ReadUInt();
            payload.Rank = data.ReadUInt();

            return payload;
        }

        public static ArraySegment<byte> Serialize(object pingOperation)
        {
            var op = pingOperation as RankChangedData;
            var data = BitBufferPool.GetInstance();
            data.Clear();
            
            data.AddUInt(op.Id);
            data.AddUInt(op.Rank);

            return data.ToArray();
        }
    }
}
