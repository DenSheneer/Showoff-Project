using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using Lean.Touch;

public class CutSceneManager : MonoBehaviour
{
    private static VideoPlayer videoPlayer = null;

    private void OnEnable()
    {
        videoPlayer = FindObjectOfType<VideoPlayer>();
        videoPlayer.loopPointReached += NextScene;
    }

    private void NextScene(VideoPlayer vp)
    {
        string sceneName = SceneManager.GetActiveScene().name;

        switch (sceneName)
        {
            case SceneLoader.CutsceneStartSceneName:
                SceneLoader.LoadScene(SceneLoader.LevelSceneName);
                break;
            case SceneLoader.CutsceneEndSceneName:
                SceneLoader.LoadScene(SceneLoader.EndScreenSceneName);
                break;
        }

    }

    public void Skipbutton()
    {
        NextScene(videoPlayer);
    }

}
