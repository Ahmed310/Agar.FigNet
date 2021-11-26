using UnityEngine;
using AgarIOCommon;
using System.Collections.Generic;

public class NetworkFoodView : MonoBehaviour
{
    public NetworkFood NetworkFood;
    public List<Color> ColorPallets;

    public void Init(NetworkFood networkFood)
    {
        NetworkFood = networkFood;
        transform.position = new Vector3(networkFood.Position.X, NetworkFood.Position.Y, 0);
        transform.rotation = Quaternion.identity;
        var sRend = GetComponent<SpriteRenderer>();
        sRend.color = ColorPallets[networkFood.ColorId];
    }
    
}
