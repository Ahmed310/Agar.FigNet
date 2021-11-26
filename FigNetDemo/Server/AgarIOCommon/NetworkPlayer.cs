using System.Numerics;

namespace AgarIOCommon
{
    public class NetworkPlayer
    {
        // id
        // name
        // color
        // position
        // int score
        // float radius 
        // int rank
        public uint Id;
        public string Name;
        public bool IsMine;
        public Vector3 Color;
        public Vector2 Position;
        public uint Rank;
        public uint Score;
    }
}
