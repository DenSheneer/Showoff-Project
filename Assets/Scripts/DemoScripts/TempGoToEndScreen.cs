using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TempGoToEndScreen : MonoBehaviour
{
    [SerializeField]
    private Button button = null;

    [SerializeField]
    private string sceneName = "EndScreen";

    private void Start()
    {
        button.onClick.AddListener(OnClickButton);
    }

    private void OnClickButton()
    {
        PlayerInfo.Score = 6969;
        PlayerInfo.Language = Language.GERMAN;

        SceneManager.LoadScene(sceneName);
    }
}
