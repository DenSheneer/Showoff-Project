using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;

public static class Highscore
{
    public static List<PlayerScore> currentDayScores;
    public static List<PlayerScore> allAroundScores;

    private static int currentDayScoreLimit = 25;
    private static int allAroundScoreLimit = 10;

    public static void LoadHighscores()
    {

        currentDayScores = new List<PlayerScore>();
        allAroundScores = new List<PlayerScore>(); 

        string loadPath = Application.persistentDataPath + "/Highscores";
        if (!Directory.Exists(loadPath))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Highscores");
        }

        string todayFileName = DateTime.Now.ToString("yyyy-MM-dd") + "-highscore.json";

        if (!File.Exists(loadPath + "/" + todayFileName))
        {
            File.Create(loadPath + "/" + todayFileName).Dispose();
        }

        string allAroundFileName = "all-around-highscore.json";

        if (!File.Exists(loadPath + "/" + allAroundFileName))
        {
            File.Create(loadPath + "/" + allAroundFileName).Dispose();
        }

        // Add empty scores
        for (int i = 0; i < currentDayScoreLimit; i++)
        {
            currentDayScores.Add(new PlayerScore("empty", DateTime.Now.ToString("yyyy-MM-dd"), 0));
        }
        for (int i = 0; i < allAroundScoreLimit; i++)
        {
            allAroundScores.Add(new PlayerScore("empty", DateTime.Now.ToString("yyyy-MM-dd"), 0));
        }


        // ############ LOADING ALL SCORES FROM THE JSON FILES ###########
        // LOADING CURRENT DAY FIRST
        string jsonString = File.ReadAllText(loadPath + "/" + todayFileName);

        JSONObject jObject = JSONObject.Create(jsonString);
        if (jObject != null && jObject.type == JSONObject.Type.OBJECT)
        {
            JSONObject scoreArray = jObject[0];
            if (jObject.Count != 0 && scoreArray != null)
            {
                for (int i = 0; i < scoreArray.Count; i++)
                {
                    string name = scoreArray[i]["name"].str;
                    int score = (int)scoreArray[i]["score"].i;
                    string date = scoreArray[i]["datePlayed"].str;

                    loadCurrentDayScore(name, score, date);
                }
            }
        }

        // AND HERE WE LOAD IN THE ALL TIMES
        jsonString = File.ReadAllText(loadPath + "/" + allAroundFileName);
        jObject = JSONObject.Create(jsonString);
        if (jObject != null && jObject.type == JSONObject.Type.OBJECT)
        {
            JSONObject scoreArray = jObject[0];
            for (int i = 0; i < scoreArray.Count; i++)
            {
                string name = scoreArray[i]["name"].str;
                int score = (int)scoreArray[i]["score"].i;
                string date = scoreArray[i]["datePlayed"].str;

                loadAllTimeScore(name, score, date);
            }
        }
        RemoveListExcess();
    }


    private static void loadCurrentDayScore(string pName, int pScore, string pDate)
    {
        PlayerScore newScore = new PlayerScore(pName, pDate, pScore);

        // Check for the current day
        int newTodayScoreIndex = NewScoreIndexCurrentDay(pScore);
        if (newTodayScoreIndex != -1)
        {
            currentDayScores.Insert(newTodayScoreIndex, newScore);
        }
    }

    private static void loadAllTimeScore(string pName, int pScore, string pDate)
    {
        PlayerScore newScore = new PlayerScore(pName, pDate, pScore);
        int newAllTimeScoreIndex = NewScoreIndexAllTime(pScore);
        // Check for the all time
        if (newAllTimeScoreIndex != -1)
        {
            allAroundScores.Insert(newAllTimeScoreIndex, newScore);
        }
    }

    public static void SaveHighscores()
    {
        // #### saving current day ####
        JSONObject jFileToday = new JSONObject(JSONObject.Type.OBJECT);
        JSONObject jScoresToday = new JSONObject(JSONObject.Type.ARRAY);
        jFileToday.AddField("scores", jScoresToday);
        for (int i = 0; i < currentDayScores.Count; i++)
        {
            JSONObject scoreObj = new JSONObject(JSONObject.Type.OBJECT);
            scoreObj.AddField("name",currentDayScores[i].name);
            scoreObj.AddField("score",currentDayScores[i].score);
            scoreObj.AddField("datePlayed",currentDayScores[i].datePlayed);
            jScoresToday.Add(scoreObj);
        }

        string loadPath = Application.persistentDataPath + "/Highscores";
        if (!Directory.Exists(loadPath))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Highscores");
        }

        string todayFileName = DateTime.Now.ToString("yyyy-MM-dd") + "-highscore.json";

        if (!File.Exists(loadPath + "/" + todayFileName))
        {
            File.Create(loadPath + "/" + todayFileName).Dispose();
        }

        File.WriteAllText(loadPath + "/" + todayFileName,string.Empty);
        File.WriteAllText(loadPath + "/" + todayFileName, jFileToday.ToString());

        // #### saving all total score ####
        JSONObject jFileOverall = new JSONObject(JSONObject.Type.OBJECT);
        JSONObject jScoresOverall = new JSONObject(JSONObject.Type.ARRAY);
        jFileOverall.AddField("scores", jScoresOverall);
        for (int i = 0; i < allAroundScores.Count; i++)
        {
            JSONObject scoreObj = new JSONObject(JSONObject.Type.OBJECT);
            scoreObj.AddField("name", allAroundScores[i].name);
            scoreObj.AddField("score", allAroundScores[i].score);
            scoreObj.AddField("datePlayed", allAroundScores[i].datePlayed);
            jScoresOverall.Add(scoreObj);
        }

        string overallFileName = "all-around-highscore.json";

        if (!File.Exists(loadPath + "/" + overallFileName))
        {
            File.Create(loadPath + "/" + overallFileName).Dispose();
        }

        File.WriteAllText(loadPath + "/" + overallFileName, string.Empty);
        File.WriteAllText(loadPath + "/" + overallFileName, jFileOverall.ToString());
    }

    public static void NewScore(string pName,int pScore,string pDate)
    {
        // Checks if name is avaliable
        if (!IsNameAvailable(pName))
            return;

        PlayerScore newScore = new PlayerScore(pName, pDate, pScore);

        // Check for the current day
        int newTodayScoreIndex = NewScoreIndexCurrentDay(pScore);
        if (newTodayScoreIndex != -1)
        {
            currentDayScores.Insert(newTodayScoreIndex, newScore);
        }

        int newAllTimeScoreIndex = NewScoreIndexAllTime(pScore);
        // Check for the all time
        if (newAllTimeScoreIndex != -1)
        {
            allAroundScores.Insert(newAllTimeScoreIndex, newScore);
        }
        RemoveListExcess();
    }

    // These functions return where the new score is should be in the list
    public static int NewScoreIndexCurrentDay(int pScore)
    {
        // For when its the first score
        if (currentDayScores.Count == 0)
            return 0;
        
        int loopCount = 0;
        if (currentDayScores.Count >= currentDayScoreLimit)
            loopCount = currentDayScoreLimit;
        else
            loopCount = currentDayScores.Count;

        for (int i = 0; i < loopCount; i++)
        {
            if (pScore >= currentDayScores[i].score)
                return i;
        }

        return -1;
    }
    
    public static int NewScoreIndexAllTime(int pScore)
    {
        // For when its the first score
        if (allAroundScores.Count == 0)
            return 0;

        int loopCount = 0;
        if (allAroundScores.Count >= allAroundScoreLimit)
            loopCount = allAroundScoreLimit;
        else
            loopCount = allAroundScores.Count;

        for (int i = 0; i < loopCount; i++)
        {
            if (pScore >= allAroundScores[i].score)
                return i;
        }

        return -1;
    }

    private static void RemoveListExcess()
    {
        if (currentDayScores.Count > currentDayScoreLimit)
        {
            currentDayScores.RemoveRange(currentDayScoreLimit - 1, currentDayScores.Count - (currentDayScoreLimit - 1));
        }

        if (allAroundScores.Count > allAroundScoreLimit)
        {
            allAroundScores.RemoveRange(allAroundScoreLimit - 1, allAroundScores.Count - (allAroundScoreLimit - 1));
        }
    }

    public static bool IsNameAvailable(string pName)
    {
        for (int i = 0; i < currentDayScores.Count; i++)
        {
            // NAME IS NOT AVAIALBE
            if (currentDayScores[i].name == pName)
                return false;
        }

        for (int i = 0; i < allAroundScores.Count; i++)
        {
            if (allAroundScores[i].name == pName)
                return false;
        }

        return true;
    }





}
