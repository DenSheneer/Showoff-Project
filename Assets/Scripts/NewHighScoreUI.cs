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

    [SerializeField]
    private EndScreenUI endScreen = null;

    private void Awake()
    {
        newDailyHighscore.gameObject.SetActive(false);
        newAllTimeHighscore.gameObject.SetActive(false);
        playerInputName.text = "";


        if (Highscore.checkHighscoreType(PlayerInfo.Score) == Highscore.HighscoreType.ALLTIME)
        {
            newAllTimeHighscore.gameObject.SetActive(true);
            
            if (PlayerInfo.Score < 10)
            {
                scoreNumber.text = "000" + PlayerInfo.Score.ToString();
            }
            else if (PlayerInfo.Score < 100)
            {
                scoreNumber.text = "00" + PlayerInfo.Score.ToString();

            }
            else if (PlayerInfo.Score < 1000)
            {
                scoreNumber.text = "0" + PlayerInfo.Score.ToString();

            }
            else
            {
                scoreNumber.text = PlayerInfo.Score.ToString();
            }
        }
        else
        {
            newDailyHighscore.gameObject.SetActive(true);
        }

        nameConfirm.onClick.AddListener(ConfirmButton);
    }

    private void ConfirmButton()
    {
        if (playerInputName.text == "")
            return;

        PlayerInfo.PlayerName = playerInputName.text;
        Highscore.NewScore(PlayerInfo.PlayerName, PlayerInfo.Score,DateTime.Now.ToString("yyyy-MM-dd"));

        endScreen.SwitchToOverview();
    }

}
