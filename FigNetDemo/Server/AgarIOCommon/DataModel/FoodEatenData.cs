using System;
using FigNet.Core;

namespace AgarIOCommon.DataModel
{
    public class FoodEatenData
    {
        private static Pool<FoodEatenData> Pool = new Pool<FoodEatenData>(() => new FoodEatenData(), (op) => op.Reset(), 4);

        public uint FoodId;
        public uint PlayerId;


        public void Reset() 
        {
            FoodId = 0;
            PlayerId = 0;
        }

        public static FoodEatenData Acquire()
        {
            return Pool.Acquire();
        }

        public static void Release(FoodEatenData obj)
        {
            Pool.Release(obj);
        }

        public static object Deserialize(ArraySegment<byte> buffer)
        {
            var data = BitBufferPool.GetInstance();
            data.FromArray(buffer);

            var payload = Acquire();

            payload.FoodId = data.ReadUInt();
            payload.PlayerId = data.ReadUInt();

            return payload;
        }

        public static ArraySegment<byte> Serialize(object pingOperation)
        {
            var op = pingOperation as FoodEatenData;
            var data = BitBufferPool.GetInstance();
            data.Clear();
            
            data.AddUInt(op.FoodId);
            data.AddUInt(op.PlayerId);

            return data.ToArray();
        }
    }
}
