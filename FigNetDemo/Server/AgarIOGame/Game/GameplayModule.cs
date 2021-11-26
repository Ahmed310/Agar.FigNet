using FigNet.Core;
using AgarIOCommon;
using AgarIOCommon.DataModel;
using System.Collections.Generic;
using AgarIOGame.Messages.Handlers;

namespace AgarIOGame.Game
{
    public class GameplayModule : IModule
    {
        private Zone gameZone;
       // private ConnectionManager connectionManager;
        private List<IHandler> handlers = new List<IHandler>() 
        {
           new JoinGameHandler(),
           new FoodEatenHandler(),
           new PlayerKilledHandler(),
           new PositionSyncHandler()
        };

        static GameplayModule()
        {
            var connectionManager = new ConnectionManager();
            FN.BindServerListner(connectionManager);
            //FN.OnSettingsLoaded += () => 
            //{
            //    var connectionManager = new ConnectionManager();
            //    FN.BindServerListner(connectionManager);
            //};
        }

        private void RegisterPayloads() 
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


        #region IModule_Implementation

        public void Load(IServer server)
        {
            FN.Logger.Info($"@GameplayModule Load");
            gameZone = new Zone();
           


            // init classes 
            // bind global classes to service locator
            // register payloads
            // register handlers
            // Q: should it make more sense to seprate-out socket events from zone??? 
            // A: Yes keep it seprate
            RegisterPayloads();
            RegisterHandlers();
            ServiceLocator.Bind(typeof(Zone), gameZone);


            //FN.SubscribeToDetailedLog((ushort)MessageId.PlayerKilled);
            //FN.SubscribeToDetailedLog((ushort)MessageId.PlayerRankChange);
        }

        public void Process(float deltaTime)
        {
            gameZone.Tick(deltaTime);
        }

        public void UnLoad()
        {
            UnRegisterHandlers();
        }
        #endregion
    }
}
