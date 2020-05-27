using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
    [SerializeField]
    Button button, BigEnterButton, SmallEnterButton, PlayGameButton;

    Vector3 originalButtonPosition;

    [SerializeField]
    KeyboardScript osk;

    private void Start()
    {
        button.onClick.AddListener(DisplayKeyboard);
        BigEnterButton.onClick.AddListener(TextEntered);
        SmallEnterButton.onClick.AddListener(TextEntered);
        PlayGameButton.onClick.AddListener(StartGame);
    }

    void DisplayKeyboard()
    {
        TextMeshProUGUI tmp = button.GetComponentInChildren<TextMeshProUGUI>();
        if (tmp != null)
        {
            button.onClick.RemoveAllListeners();

            if (tmp.text == "Enter your name...")
                tmp.text = "";

            originalButtonPosition = button.transform.localPosition;

            osk.gameObject.SetActive(true);
            osk.TargetTextField = tmp;

            button.transform.SetParent(osk.transform);
            button.transform.SetAsLastSibling();
            button.transform.localPosition += new Vector3(0, 300.0f, 0);
        }
    }
    void TextEntered()
    {
        if (checkNameValidity(osk.TargetTextField.text))
        {
            osk.gameObject.SetActive(false);
            button.transform.SetParent(osk.transform.parent);
            button.transform.localPosition = originalButtonPosition;
            button.onClick.AddListener(DisplayKeyboard);
        }
    }

    bool checkNameValidity(string name)
    {
        if (name.Length > 0)
        {
            return true;
        }else
        {
            return false;
        }
    }

    void StartGame()
    {
        PlayerInfo.PlayerName = osk.TargetTextField.text;
        SceneManager.LoadScene("DragScene");
    }
}


