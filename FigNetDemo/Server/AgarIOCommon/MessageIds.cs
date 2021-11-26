namespace AgarIOCommon
{
    public enum MessageId : ushort
    {
        JoinGame = 1,
        SpawnFood = 2,
        FoodEaten = 3,
        SpawnLocalPlayer = 4,
        SpawnRemotePlayer = 5,
        PositionSync = 6,
        PlayerKilled = 7,
        PlayerRankChange = 8,
        PlayerLeft = 9
    }
}
