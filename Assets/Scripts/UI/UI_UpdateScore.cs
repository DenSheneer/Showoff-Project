using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Animator))]
public class UI_UpdateScore : MonoBehaviour
{
    private TextMeshProUGUI scoreText = null;
    private int currentScore = 0;

    private Animator scoreChangeAnimator;
    GameObject scoreChangeEffect;


    private void Awake()
    {
        scoreChangeAnimator = GetComponent<Animator>();
        scoreText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void UpdateScore(int newScore)
    {
        int difference = newScore - currentScore;


        if (difference < 0)
        {
            scoreChangeEffect = Instantiate(Resources.Load<GameObject>("Prefabs/ScoreDown"));
            scoreChangeAnimator.SetTrigger("Damage");
            TextMeshProUGUI tmp = scoreChangeEffect.GetComponent<TextMeshProUGUI>();
            tmp.text = difference.ToString();
        }
        else if (difference > 0)
        {
            scoreChangeEffect = Instantiate(Resources.Load<GameObject>("Prefabs/ScoreUp"));
            scoreChangeAnimator.SetTrigger("Points");
            TextMeshProUGUI tmp = scoreChangeEffect.GetComponent<TextMeshProUGUI>();
            tmp.text = "+" + difference.ToString();
        }
        if (scoreChangeEffect != null)
        {

            scoreChangeEffect.transform.SetParent(transform);
            scoreChangeEffect.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
            scoreChangeEffect.transform.localScale = new Vector3(2, 2, 2);
            scoreChangeEffect.transform.localRotation = Quaternion.identity;
        }


        scoreText.text = newScore.ToString();
        currentScore = newScore;
    }
}

