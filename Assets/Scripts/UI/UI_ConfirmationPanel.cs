using UnityEngine;
using UnityEngine.UI;

public class UI_ConfirmationPanel : MonoBehaviour
{
    Button buttonYES;
    Button buttonNO;

    private void Awake()
    {
        buttonYES = GameObject.Find("YES").GetComponent<Button>();
        buttonNO = GameObject.Find("NO").GetComponent<Button>();

        buttonYES.onClick.AddListener(yesPressed);
        buttonNO.onClick.AddListener(ClosePanel);
    }

    public void ClosePanel()
    {
        UI_Manager.Unpause();
        gameObject.SetActive(false);
    }
    public void OpenPanel()
    {
        gameObject.SetActive(true);
    }
    void yesPressed()
    {
        SceneLoader.LoadScene(SceneLoader.StartScreenSceneName);
    }
}
