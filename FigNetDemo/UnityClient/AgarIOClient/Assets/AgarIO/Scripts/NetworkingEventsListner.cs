using UnityEngine;
using FigNet.Core;
using AgarIOCommon;

public class NetworkingEventsListner : MonoBehaviour, IClientSocketListener
{
    #region IClientSocketListener_Implementation
    public void OnConnected()
    {
        GameManager.GetInstance().UpdateConnectionStatus("Status: Connected");
        GameManager.GetInstance().EnableMainMenu(true);
    }

    public void OnConnectionStatusChange(PeerStatus peerStatus)
    {
    }

    public void OnDisconnected()
    {
        GameManager.GetInstance().UpdateConnectionStatus("Status: Disconnected");
    }

    public void OnInitilize(IClientSocket clientSocket)
    {
        GameManager.GetInstance().UpdateConnectionStatus("Connecting to Server...");
    }

    public void OnNetworkReceive(Message message)
    {
        bool handled = FN.HandlerCollection.HandleMessage(message, 0);
        if (!handled)
        {
            // no handler is registered against coming msg, mannually handle it here
            FN.Logger.Info($"no handler is registered against coming msgId: {message.Id}");
        }
    }

    public void OnNetworkSent(Message message, DeliveryMethod method, byte channelId = 0)
    {
    }
    #endregion
   

    void Awake()
    {
        FN.OnInitilized = () => {

            FN.Connections[0]?.BindSocketListner(this);
        };

        //FN.SubscribeToDetailedLog((ushort)MessageId.PlayerKilled);
        //FN.SubscribeToDetailedLog((ushort)MessageId.PlayerRankChange);
    }

}
