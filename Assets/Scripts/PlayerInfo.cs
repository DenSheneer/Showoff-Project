public static class PlayerInfo
{
    public static Language Language = Language.ENGLISH;
    public static Difficulty Difficulty = Difficulty.EASY;
    public static string PlayerName = "";
    public static int Score = 0;
}

public enum Difficulty { EASY = 0, MEDIUM = 1, HARD = 2 }
public enum Language { ENGLISH = 0, DUTCH = 1, GERMAN = 2 }