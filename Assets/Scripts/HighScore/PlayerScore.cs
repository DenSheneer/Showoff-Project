using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerScore
{
    public string name { get; private set; }
    public int score { get; private set; }
    public string datePlayed { get; private set; }


    public PlayerScore(string pPlayerName, string playDate, int playerScore = 0)
    {
        name = pPlayerName;
        datePlayed = playDate;
        score = playerScore;
    }
}
