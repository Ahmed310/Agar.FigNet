using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private Text TotalUsers;
    [SerializeField] private Text MyScore;
    [SerializeField] private List<LeaderboardSlot> slots;

    public void UpdateScoreLable(string score) 
    {
        MyScore.text = score;
    }

    public void UpdateTotalUsersLable(string count) 
    {
        TotalUsers.text = count;
    }

    public List<LeaderboardSlot> leaderboardSlots => slots;
}

[System.Serializable]
public class LeaderboardSlot
{
    public Text Rank;
    public Text Name;

    public void SetValues(string rank, string name) 
    {
        Rank.text = rank;
        Name.text = name;
    }

    public void SetColor(Color color)
    {
        //Rank.color = color;
        Name.color = color;
    }
}