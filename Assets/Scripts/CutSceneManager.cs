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
        SceneLoader.LoadScene(SceneLoader.LevelSceneName);
    }

    public void Skipbutton()
    {
            NextScene(videoPlayer);
    }

}
