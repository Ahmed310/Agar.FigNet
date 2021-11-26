using FigNet.Core;
using AgarIOCommon;
using AgarIOCommon.DataModel;
using UnityEngine;

public class FoodEatenHandler : IHandler
{
    public ushort MsgId => (ushort)MessageId.FoodEaten;

    public void HandleMessage(Message message, uint PeerId)
    {
        var data = message.Payload as FoodEatenData;
        // here add logic
        NetworkEntitiesContainer.RemoveFood(data.FoodId);
        // add score to player
        uint eatenBy = data.PlayerId;

        var playerView = NetworkEntitiesContainer.GetPlayerById(eatenBy);
        playerView.EatFood();

        FoodEatenData.Release(data);
    }
}