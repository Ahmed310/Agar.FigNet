using FigNet.Core;
using AgarIOCommon;
using AgarIOCommon.DataModel;

namespace AgarIOGame.Messages.Operations
{
    public class RankChangedOperation
    {
        private static readonly ushort mId = (ushort)MessageId.PlayerRankChange;
        public static Message Get(uint id, uint rank)
        {
            RankChangedData payload = RankChangedData.Acquire();
            
            payload.Id = id;
            payload.Rank = rank;

            var msg = new Message();
            msg.Id = mId;
            msg.Payload = payload;

            return msg;
        }
    }
}
