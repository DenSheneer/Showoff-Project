using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_UpdateScore : MonoBehaviour
{
    private TextMeshProUGUI scoreText = null;
    private int currentScore = 0;

    private Animator BackgroundAnimation;

    private void Start()
    {
        scoreText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void UpdateScore(int newScore)
    {
        int difference = newScore - currentScore;
        if (difference < 0)
        {
            GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/ScoreDown"));
            go.transform.SetParent(transform);
            go.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
            go.transform.localScale = new Vector3(2, 2, 2);
            go.transform.localRotation = Quaternion.identity;

            BackgroundAnimation.SetTrigger("Damage");
        }else
        {
            //  animation with PLUS score
        }

        scoreText.text = newScore.ToString();
        currentScore = newScore;
    }
}
