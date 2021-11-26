using FigNet.Core;
using AgarIOCommon;
using AgarIOCommon.DataModel;

public class SpawnFoodHandler : IHandler
{
    public ushort MsgId => (ushort)MessageId.SpawnFood;

    public void HandleMessage(Message message, uint PeerId)
    {
        var data = message.Payload as SpawnFoodData;
        // here add logic
        var netFood = new NetworkFood() 
        {
            Id = data.Id,
            Position = data.Position,
            ColorId = data.ColorId
        };

        NetworkEntitiesContainer.AddNetworkFood(data.Id, netFood);

        SpawnFoodData.Release(data);
    }
}