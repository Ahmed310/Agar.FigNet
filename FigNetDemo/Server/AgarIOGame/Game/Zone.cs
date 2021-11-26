using System;
using FigNet.Core;
using System.Linq;
using AgarIOCommon;
using System.Numerics;
using AgarIOCommon.DataModel;
using System.Collections.Generic;
using AgarIOGame.Messages.Operations;

namespace AgarIOGame.Game
{
    public class Zone : ITickable
    {
        // players:list
        // food:list

        // define helper func: addPlayer, removePlayer
        private List<NetworkFood> foods = new List<NetworkFood>();
        private List<NetworkPlayerObject> players = new List<NetworkPlayerObject>();
        private float spawnFoodCounter;
        private Random Random = new Random();
        public Zone()
        {
            // init props
            CheckFoodSpawn();
        }

        public void BroadcastMessage(Message message, DeliveryMethod delivery, byte channel = 0)
        {
            for (int i = 0; i < players.Count; i++)
            {
                players[i].Peer.SendMessage(message, delivery, channel);
            }
        }

        public void BroadcastMessage(uint peerToExclude, Message message, DeliveryMethod delivery, byte channel = 0)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (peerToExclude == players[i].Id) continue;
                players[i].Peer.SendMessage(message, delivery, channel);
            }
        }

        public void PlayerKilled(uint killed, uint killer) 
        {
            var p1 = players.Find(p => p.Id == killed);
            var p2 = players.Find(p => p.Id == killer);
            NetworkPlayerObject playerToRemove = default;
            uint _killer = 0, _killed = 0;
            bool itsATie = false;

            if (p1 == null || p2 == null || killed == killer) return;

            if (p1.NetworkPlayer.Score > p2.NetworkPlayer.Score)
            {
                _killer = killed;
                _killed = killer;

                playerToRemove = p2;
                p1.NetworkPlayer.Score += p2.NetworkPlayer.Score;
            }

            else if (p2.NetworkPlayer.Score > p1.NetworkPlayer.Score)
            {
                _killer = killer;
                _killed = killed;

                playerToRemove = p1;
                p2.NetworkPlayer.Score += p1.NetworkPlayer.Score;

            }
            else
            {
                itsATie = true;
            }
           

            if (!itsATie)
            {
                var op = PlayerKilledOperation.Get(_killed, _killer, playerToRemove.NetworkPlayer.Score);
                BroadcastMessage(op, DeliveryMethod.Reliable);
                players.Remove(playerToRemove);
                FN.Logger.Error($"killer {killed} | killed {killer}");
            }
        }

        public void FoodEaten(uint foodId, uint eaterId) 
        {
            var food = foods.Find(f=>f.Id == foodId);
            var player = players.Find(p=>p.Id == eaterId);

            foods.Remove(food);
            player.NetworkPlayer.Score++;
            var op = FoodEatenOperation.Get(foodId, eaterId);
            BroadcastMessage(op, DeliveryMethod.Reliable);
        }

        public void UpdatePosition(uint id, PositionSyncData data) 
        {
            var player = players.Find(p=>p.Id == id);

            if (player != null)
            {
                player.NetworkPlayer.Position = data.Position;
            }

            var op = PositionSyncOperation.Get(id, data.Position);

            BroadcastMessage(id, op, DeliveryMethod.Unreliable, 0);
        }

        private List<Vector2> rankList = new List<System.Numerics.Vector2>();
        private void UpdateRankOfPlayers() 
        {
            for (int i = 0; i < players.Count; i++)
            {
                rankList.Add(new Vector2(players[i].Id, players[i].NetworkPlayer.Score));
            }

            var SortedList = rankList.OrderByDescending(r => r.Y).ToList();

            for (int i = 0; i < SortedList.Count; i++)
            {
                uint pId = (uint)SortedList[i].X;
                var player = players.Find(p => p.Id == pId);
                player.NetworkPlayer.Rank = (uint)(i + 1);
                //FN.Logger.Info($"Player: {player.NetworkPlayer.Name} [{pId}] | Rank: {player.NetworkPlayer.Rank} | Score: {player.NetworkPlayer.Score}");
                var op = RankChangedOperation.Get(pId, player.NetworkPlayer.Rank);
                BroadcastMessage(op, DeliveryMethod.Reliable);
                //FN.Server.SendMessage(player.Peer, op, DeliveryMethod.Reliable, 0 );
            }

            rankList.Clear();
        }

        public void AddPlayer(IPeer peer, string name, Vector3 color, Vector2 position)
        {
            // send net msg {spawnLocalPlayer} to peer
            // send net msg {spawnRemotePlayer} to players in zone
            // send net msg {spawnRemotePlayer}s to peer
            // create new player and add it to players list
            try
            {
                // tell new player to spawn it's view
                FN.Server.SendMessage(peer, SpawnLocalPlayerOperation.Get(peer.Id, position), DeliveryMethod.Reliable, 0);

                // tell others to spawn coming player view
                foreach (var player in players)
                {
                    FN.Server.SendMessage(player.Peer, SpawnRemotePlayerOperation.Get(peer.Id, name, color, position, 99, 0), DeliveryMethod.Reliable, 0);
                }

                // tell new player to spawn existing players view
                foreach (var player in players)
                {
                    FN.Server.SendMessage(peer, SpawnRemotePlayerOperation.Get(player.Peer.Id, player.NetworkPlayer.Name, player.NetworkPlayer.Color, player.NetworkPlayer.Position, player.NetworkPlayer.Rank, player.NetworkPlayer.Score), DeliveryMethod.Reliable, 0);
                }

                var netPlayer = new NetworkPlayer()
                {
                    Id = peer.Id,
                    Color = color,
                    Position = position,
                    Name = name
                };

                var newPlayer = new NetworkPlayerObject(netPlayer, peer);

                players.Add(newPlayer);

                SendSpawnFoodOperationToComingPlayer(peer);
            }
            catch (Exception ex)
            {
                FN.Logger.Exception(ex, ex.Message);
            }
        }

        public void RemovePlayer(IPeer peer)
        {
            // remove peer from players list
            // send net msg {PlayerLeft} to players
            var playerToRemove = players.Find(p => p.Id == peer.Id);
            if (playerToRemove == null) return;

            players.Remove(playerToRemove);

            // tell others to spawn coming player view
            foreach (var player in players)
            {
                FN.Server.SendMessage(player.Peer, PlayerLeftOperation.Get(peer.Id), DeliveryMethod.Reliable, 0);
            }
        }

        // spawn food every 6 sec, 15-20
        // gameplay
        public void RemoveFoodById(uint id) 
        {
            var foodToRemove = foods.Find(f=>f.Id == id);
            if (foodToRemove != null)
            {
                foods.Remove(foodToRemove);
            }
        }

        private void SendSpawnFoodOperationToComingPlayer(IPeer peer)
        {
            for (int i = 0; i < foods.Count; i++)
            {
                var foodSpawnOperation = SpawnFoodOperation.Get(foods[i].Id, foods[i].Position, (byte)Random.Next(6));
                FN.Server.SendMessage(peer, foodSpawnOperation, DeliveryMethod.Reliable, 0);
            }
        }

        #region Object_Creation
        private static uint s_uniqueId;
        private static Random rnd = new Random();
        public static NetworkFood GetInstance()
        {
            var food = new NetworkFood();

            food.Id = s_uniqueId++;
            food.Position = new Vector2(rnd.Next(-(int)AppConstants.MAP_SIZE_X + 1, (int)AppConstants.MAP_SIZE_X - 1), rnd.Next(-(int)AppConstants.MAP_SIZE_Y + 1, (int)AppConstants.MAP_SIZE_Y - 1));

            return food;
        }
        #endregion


        private void CheckFoodSpawn() 
        {
            var foodToSpawnCount = AppConstants.MAX_FOOD_COUNT - foods.Count;

            for (int i = 0; i < foodToSpawnCount; i++)
            {
                var netFood = GetInstance();
                var foodSpawnOperation = SpawnFoodOperation.Get(netFood.Id, netFood.Position, (byte)Random.Next(6));
                foods.Add(netFood);

                for (int j = 0; j < players.Count; j++)
                {
                    FN.Server.SendMessage(players[j].Peer, foodSpawnOperation, DeliveryMethod.Reliable, 0);
                }
            }
        }


        #region ITickable_Implementation
        public void Tick(float deltaTime)
        {
            spawnFoodCounter += deltaTime;
            if (spawnFoodCounter > AppConstants.FOOD_SPAWN_INTERVAL)
            {
                spawnFoodCounter = 0;
                CheckFoodSpawn();
                UpdateRankOfPlayers();
            }


        }
        #endregion
    }
}
