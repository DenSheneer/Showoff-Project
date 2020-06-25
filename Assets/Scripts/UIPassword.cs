using UnityEngine;
using TMPro;

public class UIPassword : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI pinCodeText = null;

    [SerializeField]
    private TextMeshProUGUI responseText = null;

    private string currentCode = "";

    [SerializeField]
    private string passpin = "1234";

    private void OnEnable()
    {
        pinCodeText.text = "";
        currentCode = "";
    }

    public void NumberPressed(int number)
    {
        if (currentCode.Length >= passpin.Length)
        {
            Debug.Log("code to long");
            return;
        }

        currentCode += number.ToString();
        pinCodeText.text = currentCode;
    }

    public void DeleteNumber()
    {
        if (currentCode.Length == 0)
        {
            Debug.Log("no code pressed");
            return;
        }

        currentCode = currentCode.Remove(currentCode.Length - 1);
        pinCodeText.text = currentCode;
    }

    public void TryPin()
    {
        Debug.Log(currentCode);

        if (currentCode == passpin)
        {
            SceneLoader.LoadScene(SceneLoader.AdminPanelScene);
        }
        else
        {
            responseText.gameObject.SetActive(true);
        }
    }
}
