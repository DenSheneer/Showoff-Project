using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LeanTween_ScoreEffect : MonoBehaviour
{
    public LeanTweenType inType;
    public LeanTweenType outType;
    public float duration;
    public float delay;
    public UnityEvent onCompleteCallback;
    public RectTransform button;
    // Start is called before the first frame update
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            PointUIEffect();
        }
    }

    public void onComplete()
    {
        LeanTween.color(button.gameObject, new Color(0.0f, 0.0f, 0.0f, 0.0f), 1f).setDelay(delay);
    }

    void PointUIEffect()
    {
        LeanTween.color(button.gameObject, Color.yellow, 1f).setDelay(delay).setOnComplete(onComplete);
    }
}
