using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerInfo
{
    public static string PlayerName;
    public static int Score;
    public static Difficulty difficulty;
}

public enum Difficulty { EASY = 0, MEDIUM = 1, HARD = 2 }
