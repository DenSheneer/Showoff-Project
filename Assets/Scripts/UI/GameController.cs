using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public Text scoreCounterText;
    public Text timeRemainingText;
    public InputField playerName;
    public Button Submit;

    int totalScore = 0;
    int timeRemaining = 10;
    bool gameInPlay = true;
    Leaderboard leaderboard;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(OneSecond());
        leaderboard = GetComponent<Leaderboard>();
    }

    IEnumerator OneSecond()
    {
        while (1 == 1)
        {
            yield return new WaitForSeconds(1.0f);
            timeRemaining--;
            timeRemainingText.text = "Time: " + timeRemaining;
            if (timeRemaining <= 0)
            {
                EndGame();
                break;
            }
        }
    }

    void EndGame()
    {
        gameInPlay = false;

    }
    public void BackToStart()
    {
        SceneManager.LoadScene("UIStartscreen");
    }

    public void InitialsEntered()
    {

        if (leaderboard.CheckForHighScore(totalScore, playerName.text))
        {
            Submit.gameObject.SetActive(false);
            playerName.gameObject.SetActive(false);

        }
        else
        {
            Debug.Log("name taken");
        }


    }
    // Update is called once per frame
    void Update()//literally the very small and short game.
    {
        if (!gameInPlay)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            totalScore++;
            scoreCounterText.text = "Score: " + totalScore;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResetScene();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            int Length = leaderboard.highScoreNames.Length;
            for (int i = 0; i < Length; i++)
            {
                Debug.Log(leaderboard.highScoreNames[i]);
            }

        }
    }
    void ResetScene()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("UILeaderboard");
    }
}