using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TempGoToEndScreen : MonoBehaviour
{
    [SerializeField]
    private Button button = null;

    [SerializeField]
    private string sceneName = "EndScreen";

    [SerializeField]
    private int inputScore = 0;

    [SerializeField]
    private Language inputLanguage = Language.DUTCH;

    [SerializeField]
    private Difficulty inputDifficulty = Difficulty.EASY;

    private void Start()
    {
        button.onClick.AddListener(OnClickButton);
    }

    private void OnClickButton()
    {
        PlayerInfo.Score = inputScore;
        PlayerInfo.Language = inputLanguage;
        PlayerInfo.Difficulty = inputDifficulty;

        SceneManager.LoadScene(sceneName);
    }
}
