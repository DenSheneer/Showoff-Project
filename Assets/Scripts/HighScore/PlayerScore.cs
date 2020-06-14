using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerScore
{
    public string name { get; private set; }
    public uint score { get; private set; }
    public string datePlayed { get; private set; }


    public PlayerScore(string pPlayerName, string playDate, uint playerScore = 0)
    {
        name = pPlayerName;
        datePlayed = playDate;
        score = playerScore;
    }
}
