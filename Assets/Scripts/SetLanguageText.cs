using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class SetLanguageText : MonoBehaviour
{
    /// <summary>
    /// This array contains alls texts for the diffrent languages
    /// THIS SHOULD ALWAYS CONTAINS 3 VALUES
    /// </summary>
    [SerializeField]
    private string[] Text_ENG_DUTCH_GER = new string[3];

    private TextMeshProUGUI textGUI = null;
    private void OnEnable()
    {

        textGUI = GetComponent<TextMeshProUGUI>();

        switch (PlayerInfo.Language)
        {
            case Language.ENGLISH:
                textGUI.text = Text_ENG_DUTCH_GER[0];
                break;

            case Language.DUTCH:
                textGUI.text = Text_ENG_DUTCH_GER[1];
                break;

            case Language.GERMAN:
                textGUI.text = Text_ENG_DUTCH_GER[2];
                break;
        }
    }
}
