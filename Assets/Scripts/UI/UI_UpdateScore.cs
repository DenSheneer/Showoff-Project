using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_UpdateScore : MonoBehaviour
{
    private TextMeshProUGUI scoreText = null;
    private int currentScore = 0;

    private void Start()
    {
        scoreText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void UpdateScore(int newScore)
    {
        int difference = newScore - currentScore;
        if (difference < 0)
        {
            // animation with MINUS score
        }else
        {
            //  animation with PLUS score
        }

        scoreText.text = newScore.ToString();
        currentScore = newScore;
    }
}
