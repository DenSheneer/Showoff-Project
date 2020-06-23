using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    ConfirmationPanel confirmationPanel;
    ResetButton resetButton;
    UIFirefly uiFirefly;

    [SerializeField]
    TextMeshProUGUI tmp = null;

    float deltaTime = 0.0f;

    private void Awake()
    {
        tmp.color = new Color(0, 1, 0);
        GetComponent<Canvas>().worldCamera = Camera.main;
        resetButton = GetComponentInChildren<ResetButton>();
        confirmationPanel = GetComponentInChildren<ConfirmationPanel>();
        uiFirefly = GetComponentInChildren<UIFirefly>();

        confirmationPanel.ClosePanel();

        if (Debug.isDebugBuild == false)
            Destroy(tmp);
    }
    private void OnEnable()
    {
        resetButton.AddButtonFunction(Pause);
        resetButton.AddButtonFunction(confirmationPanel.OpenPanel);
    }

    public static void Pause()
    {
        Time.timeScale = 0;
    }
    public static void Unpause()
    {
        Time.timeScale = 1;
    }

    public void UpdateFireflyComponent(int newNrOfFlies)
    {
        uiFirefly.UpdateUI(newNrOfFlies);
    }

    private void Update()
    {
        if (tmp != null)
        {
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
            float msec = deltaTime * 1000.0f;
            float fps = 1.0f / deltaTime;
            string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);

            tmp.text = text;
        }
    }
}
