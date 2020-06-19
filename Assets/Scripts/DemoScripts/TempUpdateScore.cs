using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TempUpdateScore : MonoBehaviour
{
    private TextMeshProUGUI scoreText = null;

    private void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateScore(int newScore)
    {
        scoreText.text = newScore.ToString();
    }
}
