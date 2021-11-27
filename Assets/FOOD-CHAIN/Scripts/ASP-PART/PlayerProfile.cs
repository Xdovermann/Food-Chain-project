using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerProfile
{
    public int id;
    public string username;
    public int score;

    public bool Login;

    public PlayerProfile(string userName, int Score,bool LoginStatus)
    {
        username = userName;
        score = Score;
        Login = LoginStatus;
    }
}
