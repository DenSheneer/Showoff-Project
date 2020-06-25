using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class TogglePannel : MonoBehaviour
{
    private Button toggleBtn = null;

    [SerializeField]
    private RectTransform panelContainer = null;

    private Vector2 originalAnchorPos = new Vector2();

    [SerializeField]
    iTween.EaseType easeType = iTween.EaseType.linear;

    [SerializeField]
    private float AnimationTime = 0.5f;

    private float offSet = 10.0f;

    private void Awake()
    {
        toggleBtn = GetComponent<Button>();

        toggleBtn.onClick.AddListener(TogglePanel);

        originalAnchorPos = new Vector2(panelContainer.anchoredPosition.x, panelContainer.anchoredPosition.y - offSet);
        TogglePanel();
    }


    private void TogglePanel()
    {
        iTween.ValueTo(panelContainer.gameObject, iTween.Hash(
            "from", panelContainer.anchoredPosition,
            "to", new Vector2(originalAnchorPos.x, -originalAnchorPos.y + offSet),
            "time", AnimationTime,
            "easeType", easeType,
            "onupdatetarget", this.gameObject,
            "onupdate", "MovePannel"));

        originalAnchorPos = new Vector2(originalAnchorPos.x, -originalAnchorPos.y);
    }

    public void MovePannel(Vector2 position)
    {
        panelContainer.anchoredPosition = position;
    }
}
