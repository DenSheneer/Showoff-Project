using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenManager : MonoBehaviour
{
    public static LoadingScreenManager instance;
    public GameObject loadingScreen;
    private void Awake()
    {
        instance = this;

        SceneManager.LoadSceneAsync((int)SceneIndexes.TITLE_SCREEN, LoadSceneMode.Additive);


    }

    public void LoadCutScene()
    {
        SceneManager.UnloadSceneAsync((int)SceneIndexes.TITLE_SCREEN);
        SceneManager.LoadSceneAsync((int)SceneIndexes.CUTSCENE, LoadSceneMode.Additive);
    }

    public void LoadMainGame()
    {
        loadingScreen.SetActive(true);
        SceneManager.UnloadSceneAsync((int)SceneIndexes.CUTSCENE);
        SceneManager.LoadSceneAsync((int)SceneIndexes.GAME, LoadSceneMode.Additive);
    }
    public void EndGame()
    {
        SceneManager.UnloadSceneAsync((int)SceneIndexes.GAME);
        SceneManager.LoadSceneAsync((int)SceneIndexes.ENDING, LoadSceneMode.Additive);
    }
    public void Highscores()
    {
        SceneManager.UnloadSceneAsync((int)SceneIndexes.ENDING);
        SceneManager.LoadSceneAsync((int)SceneIndexes.HIGHSCORES, LoadSceneMode.Additive);
    }
}
