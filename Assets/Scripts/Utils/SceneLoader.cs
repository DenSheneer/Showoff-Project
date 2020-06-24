using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static readonly string StartScreenSceneName = "StartScreen_NoName";
    public static readonly string CutsceneStartSceneName = "CutScene_Start";
    public static readonly string CutsceneEndSceneName = "CutScene_End";
    public static readonly string LevelSceneName = "Prototype_Daan_NewLVLDesign";
    public static readonly string EndScreenSceneName = "EndScreen";
    public static readonly string AdminPanelScene = "AdminPanel";

    public static void LoadScene(string sceneName)
    {
        if (Application.CanStreamedLevelBeLoaded(sceneName))
            SceneManager.LoadScene(sceneName);
        else
            Debug.Log("scene does not exist or is not in build");
    }


}
