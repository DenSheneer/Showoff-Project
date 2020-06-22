using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeedbackButtons : MonoBehaviour
{
    public enum FeedBackEmotion
    {
        NORESPONSE,
        VERYUNHAPPY,
        UNHAPPY,
        NEUTRAL,
        HAPPY,
        VERYHAPPY
    }

    private FeedBackEmotion feedBackEmotion = FeedBackEmotion.NORESPONSE;
    public FeedBackEmotion SelectedEmotion { get => feedBackEmotion; }

    [SerializeField]
    private Button VeryUnHappy, UnHappy, Neutral, Happy, VeryHappy = null;

    [SerializeField]
    private GameObject selectedCircle = null;

    [SerializeField]
    private float selectedButtonScaleDown = 0.95f;

    private void Start()
    {
        VeryUnHappy.onClick.AddListener(() => { feedBackEmotion = FeedBackEmotion.VERYUNHAPPY; OnMyButtonClick(VeryUnHappy); }) ;
        UnHappy.onClick.AddListener(() => { feedBackEmotion = FeedBackEmotion.UNHAPPY; OnMyButtonClick(UnHappy); }) ;
        Neutral.onClick.AddListener(() => { feedBackEmotion = FeedBackEmotion.NEUTRAL; OnMyButtonClick(Neutral); }) ;
        Happy.onClick.AddListener(() => { feedBackEmotion = FeedBackEmotion.HAPPY; OnMyButtonClick(Happy); }) ;
        VeryHappy.onClick.AddListener(() => { feedBackEmotion = FeedBackEmotion.VERYHAPPY; OnMyButtonClick(VeryHappy); });
    }


    private void OnMyButtonClick(Button buttonClicked)
    {
        VeryUnHappy.transform.localScale = Vector3.one;
        UnHappy.transform.localScale = Vector3.one;
        Neutral.transform.localScale = Vector3.one;
        Happy.transform.localScale = Vector3.one;
        VeryHappy.transform.localScale = Vector3.one;

        buttonClicked.transform.localScale = new Vector3(selectedButtonScaleDown, selectedButtonScaleDown, 1);
        selectedCircle.transform.localPosition = buttonClicked.transform.localPosition;
    }

    public void ResetButtons()
    {
        feedBackEmotion = FeedBackEmotion.NORESPONSE;
    }
}
