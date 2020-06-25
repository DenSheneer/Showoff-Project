using UnityEngine;
using UnityEngine.UI;


public class Leaderboard : MonoBehaviour
{
    public Text[] highScores;
    int[] highScoreValues;
    public string[] highScoreNames;

    void Start()
    {   // store highscores into an array
        highScoreValues = new int[highScores.Length];
        highScoreNames = new string[highScores.Length];
        for (int x = 0; x < highScores.Length; x++)
        {       //gets values from playerfrefs and stores them in  higscorevalues.
            highScoreValues [x] = PlayerPrefs.GetInt ("highScoreValues" + x); //highScoreValues = 0
            highScoreNames [x] = PlayerPrefs.GetString ("highScoreNames" + x);
        }
        DrawScores();

    }
    void SaveScores()
    {
        for (int x = 0; x < highScores.Length; x++)
        {
            PlayerPrefs.SetInt("highScoreValues" + x, highScoreValues [x]); // reverse above, set the playerprefs to the values of the scores.
            PlayerPrefs.SetString("highScoreNames" + x, highScoreNames [x]);
        }
       // PlayerPrefs.DeleteAll
        
    }

    public bool CheckForHighScore(int _value, string _userName)
    {
        foreach (string name in highScoreNames)
        {
            if (_userName == name)
            {
                return false;
                
            }
        }
        for (int x = 0; x < highScores.Length; x++) // is our value bigger than the one that is already in the array
        {
            if (_value > highScoreValues [x])
            {   //stick the new value in there and move the rest downwards,
         
                    for (int y = highScores.Length - 1; y > x; y--)
                {
                    highScoreValues [y] = highScoreValues [y - 1];
                    highScoreNames[y] = highScoreNames[y - 1];
                }
                highScoreValues [x] = _value;
                highScoreNames[x] = _userName;
                DrawScores();
                SaveScores();
                return true;

            }
        }
        return true;
    }
  /* public void CheckForName(int _value, string _userName)
    {
        for (int x = 0; x < highScores.Length; x++) // is our value bigger than the one that is already in the array
        {
            if (_value > highScoreValues[x])
            {   //stick the new value in there and move the rest downwards,

                for (int y = highScores.Length - 1; y > x; y--)
                {
                    highScoreValues[y] = highScoreValues[y - 1];
                    highScoreNames[y] = highScoreNames[y - 1];
                }
                highScoreValues[x] = _value;
                highScoreNames[x] = _userName;
                DrawScores();
                SaveScores();
                break;
            }
        }
    }*/

    public void DrawScores()
    {
        for (int x = 0; x < highScores.Length; x++)
        {
            highScores[x].text = highScoreNames[x] + " : " + highScoreValues[x].ToString();
        }
    }
 
}
