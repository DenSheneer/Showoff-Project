using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ResetButton : MonoBehaviour
{
    Button button;
    private void OnEnable()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(resetGame);
    }
    void resetGame()
    {
        SceneLoader.LoadScene(SceneLoader.StartScreenSceneName);
    }
}
