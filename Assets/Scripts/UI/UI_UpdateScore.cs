using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_UpdateScore : MonoBehaviour
{
    private TextMeshProUGUI scoreText = null;

    private void Start()
    {
        scoreText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void UpdateScore(int newScore)
    {
        scoreText.text = newScore.ToString();
    }
}
