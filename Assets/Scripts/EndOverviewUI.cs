using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Lean.Touch;
using TMPro;

public class EndOverviewUI : MonoBehaviour
{
    [SerializeField]
    private float backToStartount = 15;
    private float counter;

    [SerializeField]
    private string beginSceneName = "StartScreen";

    [SerializeField]
    private Button backToMenu = null;

    [SerializeField]
    private TextMeshProUGUI countDownText = null;

    public void Awake()
    {
        counter = backToStartount;
        backToMenu.onClick.AddListener(BackToStart);
    }

    public void Update()
    {
        if (LeanTouch.Fingers.Count == 0)
        {
            counter -= Time.deltaTime;

            if (counter <= 0)
            {
                countDownText.text = Mathf.CeilToInt(counter).ToString();
                BackToStart();
            }
            else if (counter <= 5)
            {
                countDownText.text = Mathf.CeilToInt(counter).ToString();
                countDownText.gameObject.SetActive(true);
            }
            else
            {
                countDownText.gameObject.SetActive(false);
            }
        }
        else
        {
            counter = backToStartount;
        }
    }

    private void BackToStart()
    {
        if (Application.CanStreamedLevelBeLoaded(beginSceneName))
            SceneManager.LoadScene(beginSceneName);
    }
}
