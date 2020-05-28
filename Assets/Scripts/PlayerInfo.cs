using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerInfo
{
    public static Language Language;
    public static Difficulty Difficulty;
    public static string PlayerName;
    public static int Score;
}

public enum Difficulty { EASY = 0, MEDIUM = 1, HARD = 2 }
public enum Language { ENGLISH = 0, DUTCH = 1, GERMAN = 2 }