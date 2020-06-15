using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetGame : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene("UIStartscreen");
    }


}
