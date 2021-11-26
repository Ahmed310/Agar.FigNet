
using FigNet.Core;
using AgarIOCommon;

namespace AgarIOGame.Game
{
    public class NetworkPlayerObject
    {
        public IPeer Peer;
        public readonly NetworkPlayer NetworkPlayer;
        public readonly uint Id;
        public NetworkPlayerObject(NetworkPlayer networkPlayer, IPeer peer)
        {
            NetworkPlayer = networkPlayer;
            Peer = peer;
            Id = peer.Id;
        }
    }
}
