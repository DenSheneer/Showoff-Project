using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenuUI : MonoBehaviour
{
    public enum StartMenuState
    {
        TAPTOPLAY = 0,
        LANGUAGE = 1,
        DIFFICULTY = 2,
        TAPTOSTART = 3
    }

    private StartMenuState currentMenuState = StartMenuState.TAPTOPLAY;

    [SerializeField]
    private GameObject tapToPlayParent, langBtnParent, difficultyBtnParent, TapToStartParent;

    [SerializeField]
    private Button dutchBtn, englishBtn, germanBtn, easyBtn, mediumBtn, hardBtn, startBtn;

    [SerializeField]
    private string firstSceneName;

    private void Start()
    {
        SwitchMenuState(0);

        dutchBtn.onClick.AddListener(() => { PlayerInfo.Language = Language.DUTCH; });
        englishBtn.onClick.AddListener(() => { PlayerInfo.Language = Language.ENGLISH; });
        germanBtn.onClick.AddListener(() => { PlayerInfo.Language = Language.GERMAN; });

        easyBtn.onClick.AddListener(() => { PlayerInfo.Difficulty = Difficulty.EASY; });
        mediumBtn.onClick.AddListener(() => { PlayerInfo.Difficulty = Difficulty.MEDIUM; });
        hardBtn.onClick.AddListener(() => { PlayerInfo.Difficulty = Difficulty.HARD; });


        startBtn.onClick.AddListener(StartGame);
    }

    public void SwitchMenuState(int pState)
    {
        currentMenuState = (StartMenuState)pState;
        DisableAll();

        switch (currentMenuState)
        {
            case StartMenuState.TAPTOPLAY:
                tapToPlayParent.SetActive(true);
                break;
            case StartMenuState.LANGUAGE:
                langBtnParent.SetActive(true);
                break;
            case StartMenuState.DIFFICULTY:
                difficultyBtnParent.SetActive(true);
                break;
            case StartMenuState.TAPTOSTART:
                TapToStartParent.SetActive(true);

                break;
        }
    }

    private void Update()
    {
        if (currentMenuState == StartMenuState.TAPTOPLAY && LeanTouch.Fingers.Count > 0)
            SwitchMenuState(1);
    }

    private void StartGame()
    {
        if (Application.CanStreamedLevelBeLoaded(firstSceneName))
            SceneManager.LoadScene(firstSceneName);
    }

    private void DisableAll()
    {
        tapToPlayParent.SetActive(false);
        langBtnParent.SetActive(false);
        difficultyBtnParent.SetActive(false);
        TapToStartParent.SetActive(false);
    }

}
