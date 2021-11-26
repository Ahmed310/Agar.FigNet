using System;
using FigNet.Core;
using System.Numerics;

namespace AgarIOCommon.DataModel
{
    public class SpawnFoodData
    {
        private static Pool<SpawnFoodData> Pool = new Pool<SpawnFoodData>(() => new SpawnFoodData(), (op) => op.Reset(), 4);

        public uint Id; 
        public Vector2 Position;
        public byte ColorId;

        public void Reset() 
        {
            Id = 0;
            Position = Vector2.Zero;
        }

        public static SpawnFoodData Acquire()
        {
            return Pool.Acquire();
        }

        public static void Release(SpawnFoodData obj)
        {
            Pool.Release(obj);
        }

        public static object Deserialize(ArraySegment<byte> buffer)
        {
            var data = BitBufferPool.GetInstance();
            data.FromArray(buffer);

            var payload = Acquire();

            payload.Id = data.ReadUInt();

            payload.Position.X = data.ReadInt() / 1000f;
            payload.Position.Y = data.ReadInt() / 1000f;

            payload.ColorId = data.ReadByte();

            return payload;
        }

        public static ArraySegment<byte> Serialize(object pingOperation)
        {
            var op = pingOperation as SpawnFoodData;
            var data = BitBufferPool.GetInstance();
            data.Clear();
            
            data.AddUInt(op.Id);

            data.AddInt((int)(op.Position.X * 1000));
            data.AddInt((int)(op.Position.Y * 1000));

            data.AddByte(op.ColorId);

            return data.ToArray();
        }
    }
}
