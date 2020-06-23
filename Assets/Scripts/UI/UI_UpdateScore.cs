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
        Debug.Log("reached");
        scoreText.text = newScore.ToString();
        
        GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/ScoreUp"));
        go.transform.SetParent(transform);
        go.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
        go.transform.localScale = new Vector3(2, 2, 2);
        go.transform.localRotation = Quaternion.identity;
        Debug.Log(go.name);
    }
}
