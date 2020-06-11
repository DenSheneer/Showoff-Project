using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreListUI : MonoBehaviour
{
    [SerializeField]
    private ScoreListElement scoreElementPrefab = null;

    [SerializeField]
    private ScrollRect scrollWindowCurrentDay = null;

    [SerializeField]
    private ScrollRect scrollWindowAllAround = null;

    [SerializeField]
    private int maxScrollViewWidth = 900;
    
    void Start()
    {
        Highscore.LoadHighscores();
        Highscore.NewScore(PlayerInfo.PlayerName, PlayerInfo.Score, DateTime.Now.ToString("yyyy-MM-dd"));
        Highscore.SaveHighscores();

        // PROTOTYPEING

        //Highscore.NewScore("henk",70,"1970/01/01");
        //Highscore.NewScore("Azure", 10,"1970/01/01");
        //Highscore.NewScore("Lilly", 8,"1970/01/01");
        //Highscore.NewScore("Chantelle", 90,"1970/01/01");
        //Highscore.NewScore("Tia", 1,"1970/01/01");
        //Highscore.NewScore("Christa", 4,"1970/01/01");
        //Highscore.NewScore("Karie", 7,"1970/01/01");
        //Highscore.NewScore("Ali", 80,"1970/01/01");
        //Highscore.NewScore("Tara", 60,"1970/01/01");
        //Highscore.NewScore("Lia", 54,"1970/01/01");
        //Highscore.NewScore("Holly", 31,"1970/01/01");
        //Highscore.NewScore("Layla", 112,"1970/01/01");
        //Highscore.NewScore("Ellia", 19,"1970/01/01");
        //Highscore.NewScore("Max", 29,"1970/01/01");
        //Highscore.NewScore("Karli", 32,"1970/01/01");
        //Highscore.NewScore("Sue", 87,"1970/01/01");
        //Highscore.NewScore("Halima", 10,"1970/01/01");
        //Highscore.NewScore("Aimee", 11,"1970/01/01");
        //Highscore.NewScore("Lauren", 85,"1970/01/01");
        //Highscore.NewScore("Kara", 4,"1970/01/01");
        //Highscore.NewScore("Erika", 51,"1970/01/01");
        //Highscore.NewScore("Nora", 71,"1970/01/01");
        //Highscore.NewScore("Lyssa", 94,"1970/01/01");
        //Highscore.NewScore("Troy", 85,"1970/01/01");
        //Highscore.NewScore("Isobelle", 62,"1970/01/01");
        //Highscore.NewScore("Sally", 97,"1970/01/01");
        //Highscore.NewScore("Victoria", 39,"1970/01/01");

        // END PROTOTYPEING

        LoadList(Highscore.currentDayScores, scrollWindowCurrentDay);
        LoadList(Highscore.allAroundScores, scrollWindowAllAround);
        scrollWindowAllAround.gameObject.SetActive(false);

    }


    private void LoadList(List<PlayerScore> pPlayerScores, ScrollRect pScrollWindow)
    {
        RectTransform scrollRect = pScrollWindow.GetComponent<RectTransform>();
        scrollRect.sizeDelta = new Vector2(Mathf.Clamp((Screen.width - 400),0, maxScrollViewWidth), Screen.height - 100);

        for (int i = 0; i < pPlayerScores.Count; i++)
        {
            ScoreListElement scoreElement = Instantiate(scoreElementPrefab, pScrollWindow.content);
            scoreElement.NameText.text = pPlayerScores[i].name;
            scoreElement.ScoreText.text = pPlayerScores[i].score.ToString();
            scoreElement.DatePlayedText.text = pPlayerScores[i].datePlayed;
        }
    }

}
