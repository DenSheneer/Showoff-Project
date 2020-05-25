using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestionMenu : MonoBehaviour
{
    public void Next ()
    {
        SceneManager.LoadScene("UILeaderboard");
    }
    public void Selected()
    {
        // you cannot continue if one of the 2 emojis is not selected.
        //currently you can only select 1 emoji... 
    }
    void Start()
    {
        btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        btn.interactable = false;
    }
    // but you can only tap once and you to enable the button interaction when coming back to this page 
}
