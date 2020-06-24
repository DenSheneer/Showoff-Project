using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public const string StartScreenSceneName = "StartScreen_NoName";
    public const string CutsceneStartSceneName = "CutScene_Start";
    public const string CutsceneEndSceneName = "CutScene_End";
    public const string LevelSceneName = "Prototype_Daan_NewLVLDesign";
    public const string EndScreenSceneName = "EndScreen";
    public const string AdminPanelScene = "AdminPanel";

    public static void LoadScene(string sceneName)
    {
        if (Application.CanStreamedLevelBeLoaded(sceneName))
            SceneManager.LoadScene(sceneName);
        else
            Debug.Log("scene does not exist or is not in build");
    }


}
