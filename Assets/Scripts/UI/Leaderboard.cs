using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Leaderboard : MonoBehaviour
{
    public Text[] highScores;

    int[] highScoreValues;
    // Start is called before the first frame update
    void Start()
    {   // store highscores into an array
        highScoreValues = new int[highScores.Length];
        for (int x = 0; x < highScores.Length; x++)
        {       //gets values from playerfrefs and stores them in  higscorevalues.
            highScoreValues[x] = PlayerPrefs.GetInt("highScoreValues" + x); //highScoreValues = 0
        }
    }
    void SaveScores()
    {
        for (int x = 0; x < highScores.Length; x++)
        {
            PlayerPrefs.SetInt("highScoreValues" + x,highScoreValues [x]); // reverse above, set the playerprefs to the values of the scores.
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
