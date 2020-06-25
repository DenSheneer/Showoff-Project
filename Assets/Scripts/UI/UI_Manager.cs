using TMPro;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    UI_ConfirmationPanel uiConfirmationPanel = null;
    UI_ResetButton uiResetButton = null;
    UI_Firefly uiFirefly = null;
    UI_UpdateScore uiUpdateScore = null;

    [SerializeField]
    TextMeshProUGUI tmp = null;

    float deltaTime = 0.0f;

    private void Awake()
    {
        tmp.color = new Color(0, 1, 0);
        GetComponent<Canvas>().worldCamera = Camera.main;
        uiResetButton = GetComponentInChildren<UI_ResetButton>();
        uiConfirmationPanel = GetComponentInChildren<UI_ConfirmationPanel>();
        uiFirefly = GetComponentInChildren<UI_Firefly>();
        uiUpdateScore = GetComponentInChildren<UI_UpdateScore>();

        uiConfirmationPanel.ClosePanel();

        if (Debug.isDebugBuild == false)
            Destroy(tmp);
    }
    private void OnEnable()
    {
        uiResetButton.AddButtonFunction(Pause);
        uiResetButton.AddButtonFunction(uiConfirmationPanel.OpenPanel);
    }

    public static void Pause() { Time.timeScale = 0; }
    public static void Unpause() { Time.timeScale = 1; }
    public void UpdateFireflyComponent(int newNrOfFlies) { uiFirefly.UpdateUI(newNrOfFlies); }
    public void UpdatePlayerScore(int newScore) { uiUpdateScore.UpdateScore(newScore); }

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
