using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   
    public void PlayGame ()
    {
        SceneManager.LoadScene("UIReset");

    }

    public void LanuageGerman ()
    {
        SceneManager.LoadScene("SampleScene"); //  german version of game
    }

    public void LanguageDutch ()
    {
        SceneManager.LoadScene("SampleScene"); // dutch  version of game
    }

    public void Language_English ()
    {
        SceneManager.LoadScene("SampleScene"); //english version of game
    }
}
