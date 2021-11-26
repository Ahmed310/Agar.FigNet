
using UnityEngine;
using System.Collections.Generic;

public static class NetworkEntitiesContainer 
{
    public static Dictionary<uint, NetworkPlayerView> NetworkPlayers = new Dictionary<uint, NetworkPlayerView>();
    public static Dictionary<uint, NetworkFoodView> NetworkFoods = new Dictionary<uint, NetworkFoodView>();

    private static GameObject entitiesParent;

    public static void Init() 
    {
        if (entitiesParent == null)
            entitiesParent = new GameObject("___NETWORK_ENTITIES___");
    }

    public static NetworkPlayerView GetPlayerById(uint id)
    {
        NetworkPlayerView player = null;

        if (NetworkPlayers.ContainsKey(id))
        {
            player = NetworkPlayers[id];
        }

        return player;
    }

    public static void CleanUp() 
    {
        foreach (var player in NetworkPlayers)
        {
            GameObject.Destroy(player.Value.gameObject);
        }
        NetworkPlayers.Clear();
        
        foreach (var food in NetworkFoods)
        {
            GameObject.Destroy(food.Value.gameObject);
        }
        NetworkFoods.Clear();
    }

    public static void AddNetworkPlayer(uint id, AgarIOCommon.NetworkPlayer networkPlayer) 
    {
        if (!NetworkPlayers.ContainsKey(id))
        {
            NetworkPlayerView playerView; 
            if (networkPlayer.IsMine)
            {
                playerView = GameObject.Instantiate<NetworkPlayerView>(Resources.Load<NetworkPlayerView>("Prefabs/MyPlayer"), new Vector3() { x = networkPlayer.Position.X, y = networkPlayer.Position.Y, z = 0 }, Quaternion.identity);
                playerView.GetComponent<PlayerController>().EnableCameraFollow();
            }
            else
            {
                playerView = GameObject.Instantiate<NetworkPlayerView>(Resources.Load<NetworkPlayerView>("Prefabs/RemotePlayer"), new Vector3() { x = networkPlayer.Position.X, y = networkPlayer.Position.Y, z = 0 }, Quaternion.identity);
            }
            playerView.transform.position = new Vector3(networkPlayer.Position.X, networkPlayer.Position.Y, 0);
            playerView.transform.SetParent(entitiesParent.transform);

            playerView.Init(networkPlayer);

            NetworkPlayers.Add(id, playerView);

            GameManager.GetInstance().UpdateTotalUsers((uint)NetworkPlayers.Count);
        }
    }

    public static void RemovePlayer(uint id)
    {
        if (NetworkPlayers.ContainsKey(id))
        {
            var player = NetworkPlayers[id];
            GameObject.Destroy(player.gameObject);
            NetworkPlayers.Remove(id);

            GameManager.GetInstance().UpdateTotalUsers((uint)NetworkPlayers.Count);
        }
    }


    public static void AddNetworkFood(uint id, AgarIOCommon.NetworkFood networkFood)
    {
        if (!NetworkPlayers.ContainsKey(id))
        {
            NetworkFoodView foodView = GameObject.Instantiate<NetworkFoodView>(Resources.Load<NetworkFoodView>("Prefabs/Food"), Vector3.zero, Quaternion.identity);

            foodView.transform.SetParent(entitiesParent.transform);
            foodView.Init(networkFood);

            NetworkFoods.Add(id, foodView);
        }
    }

    public static void RemoveFood(uint id)
    {
        if (NetworkFoods.ContainsKey(id))
        {
            var food = NetworkFoods[id];
            GameObject.Destroy(food.gameObject);
            NetworkFoods.Remove(id);
        }
    }

}