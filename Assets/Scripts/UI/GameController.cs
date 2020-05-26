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


    int totalScore = 0;
    int timeRemaining = 10;
    bool gameInPlay = true;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(OneSecond ());
    }

    IEnumerator OneSecond()
    {
        while (1 == 1)
        {
            yield return new WaitForSeconds(1.0f);
            timeRemaining--;
            timeRemainingText.text = "Time: " + timeRemaining;
            if (timeRemaining <= 0) {
                EndGame ();
                break;
            }
        }
    }

    void EndGame()
    {
        gameInPlay = false;
        
    }

    public void InitialsEntered ()
    {
        GetComponent<Leaderboard>().CheckForHighScore(totalScore, playerName.text);
    }
    // Update is called once per frame
    void Update()
    {
        if (!gameInPlay)
        {
            return;
        }
        if (Input.GetKeyDown (KeyCode.Space))
        {
            totalScore++;
            scoreCounterText.text = "Score: " + totalScore;
        }
    }
}
