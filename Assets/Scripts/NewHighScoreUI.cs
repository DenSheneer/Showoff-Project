using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class NewHighScoreUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerInputName, scoreNumber, newDailyHighscore, newAllTimeHighscore = null;

    [SerializeField]
    private Button nameConfirm = null;

    private void Awake()
    {
        newDailyHighscore.gameObject.SetActive(false);
        newAllTimeHighscore.gameObject.SetActive(false);


    }
}
