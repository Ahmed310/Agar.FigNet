using FigNet.Core;
using UnityEngine;
using AgarIOCommon;
using AgarIOCommon.DataModel;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{

    private static GameManager _instance;
    public static GameManager GetInstance() 
    {
        return _instance;
    }
    
    private List<IHandler> handlers = new List<IHandler>()
    {
       new JoinGameHandler(),
       new PlayerLeftHandler(),
       new SpawnLocalPlayerHandler(),
       new SpawnRemotePlayerHandler(),
       new FoodEatenHandler(),
       new PlayerKilledHandler(),
       new PlayerRankChangedHandler(),
       new PositionSyncHandler(),
       new SpawnFoodHandler()
    };

    private void RegisterPayload() 
    {
        FN.RegisterPayload((ushort)MessageId.JoinGame, JoinGameData.Serialize, JoinGameData.Deserialize);
        FN.RegisterPayload((ushort)MessageId.PlayerLeft, PlayerLeftData.Serialize, PlayerLeftData.Deserialize);
        FN.RegisterPayload((ushort)MessageId.SpawnLocalPlayer, SpawnLocalPlayerData.Serialize, SpawnLocalPlayerData.Deserialize);
        FN.RegisterPayload((ushort)MessageId.SpawnRemotePlayer, SpawnRemotePlayerData.Serialize, SpawnRemotePlayerData.Deserialize);
        FN.RegisterPayload((ushort)MessageId.FoodEaten, FoodEatenData.Serialize, FoodEatenData.Deserialize);
        FN.RegisterPayload((ushort)MessageId.PlayerKilled, PlayerKilledData.Serialize, PlayerKilledData.Deserialize);
        FN.RegisterPayload((ushort)MessageId.PositionSync, PositionSyncData.Serialize, PositionSyncData.Deserialize);
        FN.RegisterPayload((ushort)MessageId.PlayerRankChange, RankChangedData.Serialize, RankChangedData.Deserialize);
        FN.RegisterPayload((ushort)MessageId.SpawnFood, SpawnFoodData.Serialize, SpawnFoodData.Deserialize);
    }

    private void RegisterHandlers()
    {
        foreach (var handle in handlers)
        {
            FN.HandlerCollection.RegisterHandler(handle);
        }
    }
    private void UnRegisterHandlers()
    {
        foreach (var handle in handlers)
        {
            FN.HandlerCollection.UnRegisterHandler(handle);
        }
    }

    public static string MyName = "player";
    public static Color MyColor;

    [SerializeField] private MainMenu MainMenu;
    [SerializeField] private InGameUI InGameUI;
    [SerializeField] private ConnectionStatusUI StatusUI;

    public static uint MyID;

    void Awake()
    {
        Application.targetFrameRate = 60;
        _instance = this;
        NetworkEntitiesContainer.Init();
    }

    void Start()
    {
        RegisterPayload();
        RegisterHandlers();
    }
    void OnDestroy()
    {
        UnRegisterHandlers();
    }

    public void EnableMainMenu(bool value)
    {
        MainMenu.SetUIStatus(value);
    }

    public void OnGameStarted() 
    {
        MainMenu.SetUIStatus(false);
        InGameUI.gameObject.SetActive(true);
    }

    public void OnGameEnded() 
    {
        MainMenu.SetUIStatus(true);
        InGameUI.gameObject.SetActive(false);

        NetworkEntitiesContainer.CleanUp();
    }

    List<Vector2> rankList = new List<Vector2>();
    public void UpdateLeaderBoard() 
    {
        var slots = InGameUI.leaderboardSlots;

        rankList.Clear();
        foreach (var player in NetworkEntitiesContainer.NetworkPlayers)
        {
            rankList.Add(new Vector2(player.Value.NetworkPlayer.Id, player.Value.NetworkPlayer.Rank));
        }

        var SortedList = rankList.OrderBy(r => r.y).ToList();
        bool amIInTop3 = false;
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < SortedList.Count)
            {
                var player = NetworkEntitiesContainer.GetPlayerById((uint)SortedList[i].x);
                slots[i].SetValues((i + 1).ToString(), player.NetworkPlayer.Name);
                if (player.NetworkPlayer.IsMine)
                {
                    slots[i].SetColor(Color.cyan);
                    amIInTop3 = true;
                }
                else
                {
                    slots[i].SetColor(Color.black);
                }
            }
            else
            {
                slots[i].SetColor(Color.black);
                slots[i].SetValues("x", "xxx");
            }
            
        }

        if (!amIInTop3)
        {
            slots[3].SetColor(Color.cyan);
            var myPlayer = NetworkEntitiesContainer.NetworkPlayers[MyID];
            slots[3].SetValues(myPlayer.Rank.ToString(), myPlayer.NetworkPlayer.Name);
        }
        
    }

    public void UpdateScore(uint score)
    {
        InGameUI.UpdateScoreLable($"SCORE: {score}");
    }

    public void UpdateTotalUsers(uint count)
    {
        InGameUI.UpdateTotalUsersLable($"USERS: {count}");
    }

    public void UpdateConnectionStatus(string status) 
    {
        StatusUI.SetStatus(status);
    }


    public void JoinGame() 
    {
        var joinOP = JoinGameOperation.Get(MyName, new System.Numerics.Vector3(MyColor.r, MyColor.g, MyColor.b), new System.Numerics.Vector2(0, 0));
        FN.Connections[0].SendMessage(joinOP, DeliveryMethod.Reliable);
    }
}
