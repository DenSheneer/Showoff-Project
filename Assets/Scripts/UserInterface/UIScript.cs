using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
    [SerializeField]
    Button nameTextField, BigEnterButton, SmallEnterButton, StartGameButton;

    Vector3 originalButtonPosition;

    [SerializeField]
    KeyboardScript osk;

    private void Start()
    {
        StartGameButton.gameObject.SetActive(false);
        osk.gameObject.SetActive(false);

        nameTextField.onClick.AddListener(DisplayKeyboard);
        BigEnterButton.onClick.AddListener(TextEntered);
        SmallEnterButton.onClick.AddListener(TextEntered);
        StartGameButton.onClick.AddListener(StartGame);
    }

    void DisplayKeyboard()
    {
        TextMeshProUGUI tmp = nameTextField.GetComponentInChildren<TextMeshProUGUI>();
        if (tmp != null)
        {
            if (tmp.text == "Enter your name...")                                   // Clear the sample text.
                tmp.text = "";


            osk.gameObject.SetActive(true);                                         // Enable the on-screen keyboard.
            osk.TargetTextField = tmp;                                              // Set the target textfield to the name textfield.


            originalButtonPosition = nameTextField.transform.localPosition;         // Save the original position of the name textfield before repositioning it.
            nameTextField.transform.SetParent(osk.transform);                       // Reposition the name textfield to the top of the on-screen keyboard.
            nameTextField.transform.SetAsLastSibling();
            nameTextField.transform.localPosition += new Vector3(0, 300.0f, 0);     
            nameTextField.interactable = false;
        }
    }
    void TextEntered()
    {
        if (checkNameValidity(osk.TargetTextField.text))
        {
            StartGameButton.gameObject.SetActive(true);                             // Enable the Play Game button.
            nameTextField.interactable = true;                                      // Make the text field interactable again.
            osk.gameObject.SetActive(false);                                        // Disable the on-Screen keyboard.

            nameTextField.transform.SetParent(osk.transform.parent);                // Reposition the name textfield to it's original position.
            nameTextField.transform.localPosition = originalButtonPosition;
            nameTextField.transform.SetAsLastSibling();
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


