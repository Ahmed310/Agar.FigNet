using System;
using FigNet.Core;
using System.Numerics;

namespace AgarIOCommon.DataModel
{
    public class JoinGameData
    {
        public string Name; 
        public Vector3 Color;
        public Vector2 Position;

        public static JoinGameData Acquire()
        {
            return new JoinGameData();
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
