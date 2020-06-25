using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class AdminPanel : MonoBehaviour
{
    [SerializeField]
    private Button exportbt,backToGame = null;

    private void Start()
    {
        exportbt.onClick.AddListener(ExportLists);
        backToGame.onClick.AddListener(() => { SceneLoader.LoadScene(SceneLoader.StartScreenSceneName); });
    }

    private void ExportLists()
    {
        string loadPathFeedback = Application.persistentDataPath + "/Feedback/";
        if (!Directory.Exists(loadPathFeedback))
        {
            Directory.CreateDirectory(loadPathFeedback);
        }
        string filePathFeedback = loadPathFeedback + "feedback.json";
        if (!File.Exists(filePathFeedback))
        {
            File.Create(filePathFeedback).Dispose();
        }

        CSVFeedback(filePathFeedback);

        string highscoreDirectory = Application.persistentDataPath + "/Highscores";

        foreach(string filePath in Directory.GetFiles(highscoreDirectory))
        {
            CVSHighscore(filePath);
        }

        string dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        dir += "\\" + Application.productName;
 
        if (Directory.Exists(dir))
        {
            Process.Start("explorer.exe", dir);
        }
    }

    private void CVSHighscore(string filePathJson)
    {
        string fileName = Path.GetFileNameWithoutExtension(filePathJson);
        fileName += ".csv";
        string CSVpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        CSVpath += "\\" + Application.productName + "\\highscores\\";
        string filePath = CSVpath + fileName;

        if (!Directory.Exists(CSVpath))
        {
            Directory.CreateDirectory(CSVpath);
        }

        if (!File.Exists(filePath))
        {
            File.Create(filePath).Dispose();
        }
        else
        {
            File.WriteAllText(filePath, string.Empty);
        }

        string jsonString = File.ReadAllText(filePathJson);
        JSONObject jObject = JSONObject.Create(jsonString);

        if (jObject != null && jObject.type == JSONObject.Type.OBJECT)
        {
            JSONObject scoreArray = jObject[0];
            if (jObject.Count != 0 && scoreArray != null)
            {
                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine("Name,Score,Date");
                    
                    for (int i = 0; i < scoreArray.Count; i++)
                    {
                        sw.WriteLine(scoreArray[i]["name"].str + "," + scoreArray[i]["score"].i.ToString() + "," + scoreArray[i]["datePlayed"].str);
                    }
                }
            }
        }
    }

    private void CSVFeedback(string filePathJson)
    {
        string CSVpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        CSVpath += "\\" + Application.productName + "\\";
        string filePath = CSVpath + "feedback.csv";

        if (!Directory.Exists(CSVpath))
        {
            Directory.CreateDirectory(CSVpath);
        }

        if (!File.Exists(filePath))
        {
            File.Create(filePath).Dispose();
        }
        else
        {
            File.WriteAllText(filePath, string.Empty);
        }

        string jsonString = File.ReadAllText(filePathJson);
        JSONObject jObject = JSONObject.Create(jsonString);

        if (jObject != null && jObject.type == JSONObject.Type.OBJECT)
        {
            foreach (string questionKey in jObject.keys)
            {
                string FirstEntry = questionKey;
                FirstEntry += ",VeryUnHappy,UnHappy,Neutral,Happy,VeryHappy";
                string SecondEntry = "Count :,";
                SecondEntry += (jObject[questionKey]["VeryUnHappyCount"].i).ToString() + "," + (jObject[questionKey]["UnHappyCount"].i).ToString() + "," + (jObject[questionKey]["NeutralCount"].i).ToString() + "," + (jObject[questionKey]["HappyCount"].i).ToString() + "," + (jObject[questionKey]["VeryHappyCount"].i).ToString();

                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine(FirstEntry);
                    sw.WriteLine(SecondEntry);
                }
            }
        }
    }
}
