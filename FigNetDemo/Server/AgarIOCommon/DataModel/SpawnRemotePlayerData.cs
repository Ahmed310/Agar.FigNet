using System;
using FigNet.Core;
using System.Numerics;


namespace AgarIOCommon.DataModel
{
    public class SpawnRemotePlayerData
    {
        public uint Id;
        public string Name;
        public Vector3 Color;
        public Vector2 Position;
        public uint Rank;
        public uint Score;

       
        public static SpawnRemotePlayerData Acquire()
        {
            return new SpawnRemotePlayerData();
        }

        public static object Deserialize(ArraySegment<byte> buffer)
        {
            var data = BitBufferPool.GetInstance();
            data.FromArray(buffer);

            var payload = Acquire();

            payload.Id = data.ReadUInt();
            payload.Name = data.ReadString();

            payload.Color.X = data.ReadInt() / 1000f;
            payload.Color.Y = data.ReadInt() / 1000f;
            payload.Color.Z = data.ReadInt() / 1000f;

            payload.Position.X = data.ReadInt() / 1000f;
            payload.Position.Y = data.ReadInt() / 1000f;

            payload.Rank = data.ReadUInt();
            payload.Score = data.ReadUInt();

            return payload;
        }

        public static ArraySegment<byte> Serialize(object pingOperation)
        {
            var op = pingOperation as SpawnRemotePlayerData;
            var data = BitBufferPool.GetInstance();
            data.Clear();

            data.AddUInt(op.Id);
            data.AddString(op.Name);

            data.AddInt((int)(op.Color.X * 1000));
            data.AddInt((int)(op.Color.Y * 1000));
            data.AddInt((int)(op.Color.Z * 1000));

            data.AddInt((int)(op.Position.X * 1000));
            data.AddInt((int)(op.Position.Y * 1000));

            data.AddUInt(op.Rank);
            data.AddUInt(op.Score);

            return data.ToArray();
        }
    }
}
