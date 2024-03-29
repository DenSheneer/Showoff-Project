﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class NextSceneButton : MonoBehaviour
{
    private readonly static string sceneName = "StartScreen_NoName";
    
    private Button btn = null;

    private void Start()
    {
        btn = GetComponent<Button>();

        btn.onClick.AddListener(NextScene);
    }

    private void NextScene()
    {
        if (Application.CanStreamedLevelBeLoaded(sceneName))
            SceneManager.LoadScene(sceneName);
        else
            Debug.Log("scene does not exist or is not in build");
    }
}
