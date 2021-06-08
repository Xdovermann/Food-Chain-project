using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerProfile
{
    public int id;
    public string username;
    public int score;

    public PlayerProfile(string userName, int Score)
    {
        username = userName;
        score = Score;
    }
}
