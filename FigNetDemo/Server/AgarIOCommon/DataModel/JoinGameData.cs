using System;
using FigNet.Core;
using System.Numerics;

namespace AgarIOCommon.DataModel
{
    public class JoinGameData
    {
        private static Pool<JoinGameData> Pool = new Pool<JoinGameData>(() => new JoinGameData(), (op) => op.Reset(), 4);

        public string Name; 
        public Vector3 Color;
        public Vector2 Position;


        public void Reset() 
        {
            Name = "";
            Color = Vector3.Zero;
            Position = Vector2.Zero;
        }

        public static JoinGameData Acquire()
        {
            return Pool.Acquire();
        }

        public static void Release(JoinGameData obj)
        {
            Pool.Release(obj);
        }

        public static object Deserialize(ArraySegment<byte> buffer)
        {
            var data = BitBufferPool.GetInstance();
            data.FromArray(buffer);

            var payload = Acquire();     
            
            payload.Name = data.ReadString();
            
            payload.Color.X = data.ReadInt() / 1000f;
            payload.Color.Y = data.ReadInt() / 1000f;
            payload.Color.Z = data.ReadInt() / 1000f;

            payload.Position.X = data.ReadInt() / 1000f;
            payload.Position.Y = data.ReadInt() / 1000f;

            return payload;
        }

        public static ArraySegment<byte> Serialize(object pingOperation)
        {
            var op = pingOperation as JoinGameData;
            var data = BitBufferPool.GetInstance();
            data.Clear();
            
            data.AddString(op.Name);

            data.AddInt((int)(op.Color.X * 1000));
            data.AddInt((int)(op.Color.Y * 1000));
            data.AddInt((int)(op.Color.Z * 1000));

            data.AddInt((int)(op.Position.X * 1000));
            data.AddInt((int)(op.Position.Y * 1000));

            return data.ToArray();
        }
    }
}
