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
        //TAPTOSTART = 3
    }

    private StartMenuState currentMenuState = StartMenuState.TAPTOPLAY;

    [SerializeField]
    private GameObject tapToPlayParent, langBtnParent, difficultyBtnParent;

    [SerializeField]
    private Button dutchBtn, englishBtn, germanBtn, easyBtn, mediumBtn, hardBtn, toStartBtn;

    FloatingBehaviour fb = new FloatingBehaviour(0.25f, 1, 1.10f);
    Vector3 originalScale;

    private void Awake()
    {
        originalScale = tapToPlayParent.transform.localScale;
    }

    private void Start()
    {
        SwitchMenuState(0);

        toStartBtn.onClick.AddListener(() => { EnableParent(langBtnParent); });

        dutchBtn.onClick.AddListener(() => { PlayerInfo.Language = Language.DUTCH; EnableParent(difficultyBtnParent); });
        englishBtn.onClick.AddListener(() => { PlayerInfo.Language = Language.ENGLISH; EnableParent(difficultyBtnParent); });
        germanBtn.onClick.AddListener(() => { PlayerInfo.Language = Language.GERMAN; EnableParent(difficultyBtnParent); });

        easyBtn.onClick.AddListener(() => { PlayerInfo.Difficulty = Difficulty.EASY; });
        mediumBtn.onClick.AddListener(() => { PlayerInfo.Difficulty = Difficulty.MEDIUM; });
        hardBtn.onClick.AddListener(() => { PlayerInfo.Difficulty = Difficulty.HARD; });

        easyBtn.onClick.AddListener(StartGame);
        mediumBtn.onClick.AddListener(StartGame);
        hardBtn.onClick.AddListener(StartGame);
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
            //case StartMenuState.TAPTOSTART:
            //    TapToStartParent.SetActive(true);

            //    break;
        }
    }

    private void FixedUpdate()
    {
        tapToPlayParent.transform.localScale = originalScale * fb.GetScaleFactor();
    }

    private void StartGame()
    {
        SceneLoader.LoadScene(SceneLoader.CutsceneStartSceneName);
    }

    private void DisableAll()
    {
        tapToPlayParent.SetActive(false);
        langBtnParent.SetActive(false);
        difficultyBtnParent.SetActive(false);
        //TapToStartParent.SetActive(false);
    }

    private void EnableParent(GameObject parent)
    {
        DisableAll();
        parent.SetActive(true);
    }

}
