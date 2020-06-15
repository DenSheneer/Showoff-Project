using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndScreenUI : MonoBehaviour
{
    [SerializeField]
    private NewHighScoreUI newHighscoreUI = null;

    [SerializeField]
    private EndOverviewUI endOverviewUI = null;

    private void Awake()
    {
        newHighscoreUI.gameObject.SetActive(false);
        endOverviewUI.gameObject.SetActive(false);

        if (Highscore.checkHighscoreType(PlayerInfo.Score) == Highscore.HighscoreType.ALLTIME || Highscore.checkHighscoreType(PlayerInfo.Score) == Highscore.HighscoreType.DAILY)
        {
            newHighscoreUI.gameObject.SetActive(true);
        }
        else
        {
            endOverviewUI.gameObject.SetActive(true);
        }
    }
}
