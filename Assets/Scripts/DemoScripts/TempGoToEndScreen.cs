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
        Highscore.LoadHighscores();
        Highscore.NewScore("Wessel",0,"2020/06/11");
        Highscore.NewScore("Wessel Bus",9,"2020/06/11");
        Highscore.NewScore("Wessel Zus",81,"2020/06/11");
        Highscore.NewScore("Wessel Kus",714,"2020/06/11");
        Highscore.NewScore("Wessel Rus",5214,"2020/06/11");
        Highscore.SaveHighscores();

        SceneManager.LoadScene(sceneName);
    }
}
