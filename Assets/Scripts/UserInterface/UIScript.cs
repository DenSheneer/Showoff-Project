using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Lean.Touch;

public class UIScript : MonoBehaviour
{
    [SerializeField]
    GameObject pressToPlayText;

    [SerializeField]
    Button nameTextField = null, BigEnterButton = null, SmallEnterButton = null, StartGameButton = null;

    [SerializeField]
    Button[] languageButtons = new Button[3], difficultyButtons = new Button[3];

    bool firstFrameAfterSwitch = true;

    Vector3 originalButtonPosition;

    [SerializeField]
    KeyboardScript osk;

    StartScreenState currentState = StartScreenState.PRESSTOSTART;

    private void Start()
    {
        disableAll();

        nameTextField.onClick.AddListener(DisplayKeyboard);
        BigEnterButton.onClick.AddListener(TextEntered);
        SmallEnterButton.onClick.AddListener(TextEntered);
        StartGameButton.onClick.AddListener(StartGame);
    }

    private void Update()
    {
        switch (currentState)
        {
            case StartScreenState.PRESSTOSTART:
                pressToPlay();
                break;
            case StartScreenState.SELECTLANGUAGE:
                selectlanguage();
                break;
            case StartScreenState.SELECTDIFFICULTY:
                selectDifficulty();
                break;
            case StartScreenState.ENTERNAME:
                enterName();
                break;
            case StartScreenState.STARTLEVEL:
                break;
        }
    }

    void pressToPlay()
    {
        if (firstFrameAfterSwitch)
        {
            firstFrameAfterSwitch = false;
            pressToPlayText.SetActive(true);
        }
        if (LeanTouch.Fingers.Count > 0)
            changeState((int)currentState + 1);
    }
    void selectlanguage()
    {
        if (firstFrameAfterSwitch)
        {
            foreach (Button button in languageButtons)
                button.gameObject.SetActive(true);

            firstFrameAfterSwitch = false;
        }
        return;
    }
    void selectDifficulty()
    {
        if (firstFrameAfterSwitch)
        {
            foreach (Button button in difficultyButtons)
                button.gameObject.SetActive(true);

            firstFrameAfterSwitch = false;
        }
        return;
    }

    void enterName()
    {
        if (firstFrameAfterSwitch)
        {
            nameTextField.gameObject.SetActive(true);
            firstFrameAfterSwitch = false;
        }
        return;
        
    }
    public void SetLanguage(int langInt = 0)
    {
        PlayerInfo.Language = (Language)langInt;
        changeState((int)currentState + 1);
    }
    public void SetDifficulty(int difInt = 1)
    {
        PlayerInfo.Difficulty = (Difficulty)difInt;
        changeState((int)currentState + 1);
    }


    void DisplayKeyboard()
    {
        StartGameButton.gameObject.SetActive(false);
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
    public void TextEntered()
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
        changeState((int)currentState + 1);
        PlayerInfo.PlayerName = osk.TargetTextField.text;
        SceneManager.LoadScene("DragScene");
    }
    void changeState(int nextStage)
    {
        disableAll();
        currentState = (StartScreenState)nextStage;
        firstFrameAfterSwitch = true;
    }
    void disableAll()
    {
        pressToPlayText.SetActive(false);
        nameTextField.gameObject.SetActive(false);
        osk.gameObject.SetActive(false);
        StartGameButton.gameObject.SetActive(false);

        foreach (Button button in languageButtons)
            button.gameObject.SetActive(false);
        foreach (Button button in difficultyButtons)
            button.gameObject.SetActive(false);
    }
}


public enum StartScreenState
{
    PRESSTOSTART = 0,
    SELECTLANGUAGE = 1,
    SELECTDIFFICULTY = 2,
    ENTERNAME = 3,
    STARTLEVEL = 4
}


