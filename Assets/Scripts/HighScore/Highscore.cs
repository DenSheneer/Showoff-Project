using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public static class Highscore
{
    // <FileName,ListOfPlayerScores> 
    public static Dictionary<Difficulty, List<PlayerScore>> dailyScoreLists;
    public static Dictionary<Difficulty, List<PlayerScore>> allTimeScoreLists;

    // The score limit is 10 and 25 but because of array indexing we need to add one to both values. This way its easier than adding 1 everywhere and that might add bugs aswell
    private static int currentDayScoreLimit = 10;
    private static int allAroundScoreLimit = 25;

    private static bool scoreIsLoading = false;

    public enum ScoreType
    {
        DAILY,
        ALLTIME,
        NONE
    }

    public static void LoadHighscores()
    {
        if (scoreIsLoading)
            return;

        scoreIsLoading = true;

        dailyScoreLists = new Dictionary<Difficulty, List<PlayerScore>>();
        allTimeScoreLists = new Dictionary<Difficulty, List<PlayerScore>>();

        string loadPath = Application.persistentDataPath + "/Highscores/";
        if (!Directory.Exists(loadPath))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Highscores/");
        }

        // Make/Load daily lists
        foreach (Difficulty diff in (Difficulty[]) Enum.GetValues(typeof(Difficulty)))
        {
            string difString = diff.ToString();
            string type = DateTime.Now.ToString("yyyy-MM-dd");
            string filePath = loadPath + type + "-" + difString + "-highscore.json";

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Dispose();
            }

            makeHighscoreList(filePath, currentDayScoreLimit, diff, ScoreType.DAILY);
        }

        // Make/Load all time lists
        foreach (Difficulty diff in (Difficulty[])Enum.GetValues(typeof(Difficulty)))
        {
            string difString = diff.ToString();
            string type = "all-around";
            string filePath = loadPath + type + "-" + difString + "-highscore.json";

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Dispose();
            }

            makeHighscoreList(filePath, allAroundScoreLimit, diff, ScoreType.ALLTIME);
        }

        //foreach(PlayerScore playerScore in dailyScoreLists[Difficulty.EASY])
        //{
        //    Debug.Log(playerScore.name);
        //}
    }

    private static void makeHighscoreList(string pFilePath,int scoreLimit, Difficulty pDifficulty, ScoreType pScoreType)
    {
        List<PlayerScore> playerScores = new List<PlayerScore>();

        // Add empty entries to the score to fill it up
        for (int i = 0; i < scoreLimit; i++)
            playerScores.Add(new PlayerScore("empty", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), 0));

        string JsonString = File.ReadAllText(pFilePath);
        JSONObject jObject = JSONObject.Create(JsonString);
        if (jObject != null && jObject.type == JSONObject.Type.OBJECT)
        {
            JSONObject scoreArray = jObject[0];
            if (jObject.Count != 0 && scoreArray != null)
            {
                for (int i = 0; i < scoreArray.Count; i++)
                {
                    string name = scoreArray[i]["name"].str;
                    int score = (int)Convert.ToUInt32(scoreArray[i]["score"].i);
                    string date = scoreArray[i]["datePlayed"].str;

                    addListEntry(playerScores, scoreLimit, new PlayerScore(name,date,score), pDifficulty, pScoreType);
                }
            }
        }

        RemoveListExcess(playerScores, scoreLimit);

        switch (pScoreType)
        {
            case ScoreType.DAILY:
                dailyScoreLists[pDifficulty] = playerScores;
                break;
            case ScoreType.ALLTIME:
                allTimeScoreLists[pDifficulty] = playerScores;
                break;
        }
    }

    private static void addListEntry(List<PlayerScore>pList,int pScoreLimit, PlayerScore pPlayerScore, Difficulty pDifficulty, ScoreType pScoreType)
    {
        int listIndex = NewScoreIndex(pPlayerScore.score, pList, pScoreLimit);
        
        if (listIndex != -1)
        {
            pList.Insert(listIndex, pPlayerScore);
        }
    }

    public static void NewScore(string pName, int pScore,string pDate,Difficulty pDifficulty)
    {
        if (scoreIsLoading == false)
            LoadHighscores();

        PlayerScore newScore = new PlayerScore(pName, pDate, pScore);


        int newTodayScoreIndex = NewScoreIndex(newScore.score, dailyScoreLists[pDifficulty], currentDayScoreLimit);

        if (newTodayScoreIndex != -1)
        {
            dailyScoreLists[pDifficulty].Insert(newTodayScoreIndex, newScore);
            RemoveListExcess(dailyScoreLists[pDifficulty],currentDayScoreLimit);
        }

        int newAllTimeScoreIndex = NewScoreIndex(newScore.score, allTimeScoreLists[pDifficulty], allAroundScoreLimit);
        if (newAllTimeScoreIndex != -1)
        {
            allTimeScoreLists[pDifficulty].Insert(newAllTimeScoreIndex, newScore);
            RemoveListExcess(allTimeScoreLists[pDifficulty], allAroundScoreLimit);
        }

        SaveHighscores();
    }

    public static int NewScoreIndex(int pPlayerScore, List<PlayerScore> pList, int pScoreLimit)
    {
        if (pList.Count == 0)
            return 0;

        int loopCount = 0;

        if (pList.Count >= pScoreLimit)
            loopCount = pScoreLimit;
        else
            loopCount = pList.Count;

        for (int i = 0; i < loopCount; i++)
        {
            if (pPlayerScore >= pList[i].score)
            {

                return i;
            }
        }

        // No new index was given
        return -1;
    }

    public static void SaveHighscores()
    {
        if (scoreIsLoading == false)
            LoadHighscores();

        string loadPath = Application.persistentDataPath + "/Highscores/";

        // Make/Load daily lists
        foreach (Difficulty diff in (Difficulty[])Enum.GetValues(typeof(Difficulty)))
        {
            string difString = diff.ToString();
            string type = DateTime.Now.ToString("yyyy-MM-dd");
            string filePath = loadPath + type + "-" + difString + "-highscore.json";

            SaveListToFile(filePath, dailyScoreLists[diff]);
        }

        // Make/Load all time lists
        foreach (Difficulty diff in (Difficulty[])Enum.GetValues(typeof(Difficulty)))
        {
            string difString = diff.ToString();
            string type = "all-around";
            string filePath = loadPath + type + "-" + difString + "-highscore.json";

            SaveListToFile(filePath, allTimeScoreLists[diff]);
        }

    }

    private static void SaveListToFile(string pFilePath,List<PlayerScore> pList)
    {
        JSONObject jFile = new JSONObject(JSONObject.Type.OBJECT);
        JSONObject jScores = new JSONObject(JSONObject.Type.ARRAY);
        jFile.AddField("scores", jScores);

        for (int i = 0; i < pList.Count; i++)
        {
            JSONObject scoreObj = new JSONObject(JSONObject.Type.OBJECT);
            scoreObj.AddField("name", pList[i].name);
            scoreObj.AddField("score", pList[i].score);
            scoreObj.AddField("datePlayed", pList[i].datePlayed);
            jScores.Add(scoreObj);
        }

        File.WriteAllText(pFilePath, String.Empty);
        File.WriteAllText(pFilePath, jFile.ToString());
    }

    private static void RemoveListExcess(List<PlayerScore> pList,int listLimit)
    {
        while (pList.Count > listLimit)
            pList.RemoveAt(pList.Count - 1);
    }

    public static ScoreType getHighscoreType(int pScore,Difficulty pDifficulty)
    {
        if (NewScoreIndex(pScore, allTimeScoreLists[pDifficulty], allAroundScoreLimit) != -1)
            return ScoreType.ALLTIME;
        else if (NewScoreIndex(pScore, dailyScoreLists[pDifficulty], currentDayScoreLimit) != -1)
            return ScoreType.DAILY;

        return ScoreType.NONE;
    }
}
