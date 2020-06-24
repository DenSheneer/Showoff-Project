using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Lean.Touch;
using System;

public class AfkDetector : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timeoutCounterText = null;

    [SerializeField]
    private int secondsToAFKWarning = 30;

    [SerializeReference]
    private int secondsToGameReset = 10;

    [SerializeField]
    private GameObject afkPannel;

    private float timeWithoutInput;
    private bool isAfkPannelActive;

    private float timeInAfkPannel;


    private void Start()
    {
        afkPannel.SetActive(false);
        isAfkPannelActive = false;
    }


    private void Update()
    {
        if (LeanTouch.Fingers.Count == 0)
            timeWithoutInput += Time.deltaTime;
        else
            timeWithoutInput = 0;

        if (timeWithoutInput >= secondsToAFKWarning)
        {
            afkPannel.SetActive(true);
            isAfkPannelActive = true;
        }

        if (isAfkPannelActive)
        {
            timeInAfkPannel += Time.deltaTime;
            timeoutCounterText.text = Mathf.CeilToInt(secondsToGameReset - timeInAfkPannel).ToString();

        }

        if (timeInAfkPannel >= secondsToGameReset)
            ResetGame();
    }

    public void ResetGame()
    {
        SceneLoader.LoadScene(SceneLoader.StartScreenSceneName);
    }
}
