using FigNet.Core;
using AgarIOCommon;
using AgarIOCommon.DataModel;

public class JoinGameHandler : IHandler
{
    public ushort MsgId => (ushort)MessageId.JoinGame;

    public void HandleMessage(Message message, uint PeerId)
    {
        var joinGameData = message.Payload as JoinGameData;
        // here add logic
        GameManager.GetInstance().OnGameStarted();
        FN.Logger.Info($"On Game Joined...");

    }
}