using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Image))]
public class StartMessage : MonoBehaviour
{
    private Image background = null;

    [SerializeField]
    private TextMeshProUGUI startText = null;

    private Renderer myRenderer = null;

    private void Start()
    {
        background = GetComponent<Image>();
        myRenderer = GetComponent<Renderer>();

        FadeOut();
    }


    private void Update()
    {

    }


    public void FadeOut()
    {
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", 1.0f, "to", 0.0f,
            "delay", 2.0f,
            "time", 3f, "easetype", "linear",
            "onupdate", "setAlpha"));

    }

    public void setAlpha(float newAlpha)
    {
        background.color = new Color(0,0,0,newAlpha);
        startText.color = new Color(225, 225, 225, newAlpha);

        if (background.color.a <= 0)
            gameObject.SetActive(false);
    }
}
