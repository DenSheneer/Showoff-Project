using UnityEngine;
using TMPro;

public class FeedbackQuestion : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI questionText = null;

    [SerializeField]
    private FeedbackButtons feedbackButtons = null;

    public string GetQuestion()
    {
        if (questionText != null)
        {
            return questionText.text;
        }
        else
        {
            return "Something went wrong in making this question";
        }
    }

    public FeedbackButtons.FeedBackEmotion FeedbackEmotion()
    {
        if (feedbackButtons != null)
            return feedbackButtons.SelectedEmotion;
        else
            return FeedbackButtons.FeedBackEmotion.NORESPONSE;
    }

    public void ResetButton()
    {
        if (feedbackButtons != null)
            feedbackButtons.ResetButtons();
    }
}
