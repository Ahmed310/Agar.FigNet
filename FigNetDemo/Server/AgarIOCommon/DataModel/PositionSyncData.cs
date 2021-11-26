using System;
using FigNet.Core;
using System.Numerics;

namespace AgarIOCommon.DataModel
{
    public class PositionSyncData
    {
        private static Pool<PositionSyncData> Pool = new Pool<PositionSyncData>(() => new PositionSyncData(), (op) => op.Reset(), 64);

        public uint Id; 
        public Vector2 Position;


        public void Reset() 
        {
            Id = 0;
            Position = Vector2.Zero;
        }

        public static PositionSyncData Acquire()
        {
            return Pool.Acquire();
        }

        public static void Release(PositionSyncData obj)
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

            return payload;
        }

        public static ArraySegment<byte> Serialize(object pingOperation)
        {
            var op = pingOperation as PositionSyncData;
            var data = BitBufferPool.GetInstance();
            data.Clear();
            
            data.AddUInt(op.Id);

            data.AddInt((int)(op.Position.X * 1000));
            data.AddInt((int)(op.Position.Y * 1000));

            return data.ToArray();
        }
    }
}
