﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using Lean.Touch;

public class CutSceneManager : MonoBehaviour
{
    [SerializeField]
    private VideoPlayer videoPlayer = null;

    [SerializeField]
    private string nextSceneName = "Prototype_Daan";

    private void Start()
    {
        videoPlayer.loopPointReached += NextScene;
    }

    private void NextScene(VideoPlayer vp)
    {
        if (Application.CanStreamedLevelBeLoaded(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
        else
            Debug.Log("scene does not exist or is not in build");
    }

    private void Update()
    {
        if (LeanTouch.Fingers.Count > 0)
            NextScene(videoPlayer);

    }

}
