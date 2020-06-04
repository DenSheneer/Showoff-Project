using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TutorialIcon
{
    static string imgPath = "Textures/UI/";

    GameObject targetObject;
    Transform transform;

    Vector3 originalScale;
    FloatingBehaviour floatingBehaviour = new FloatingBehaviour(0.25f, 1f, 1.1f);

    public TutorialIcon(Transform pParent, TutorialType pTutorialType)
    {
        targetObject = Object.Instantiate(Resources.Load("Prefabs/TutorialCanvas") as GameObject);
        transform = targetObject.transform;

        transform.SetParent(pParent);
        transform.localPosition = new Vector3(0, pParent.transform.lossyScale.y, 0);
        transform.localScale = pParent.transform.lossyScale * 0.75f;
        originalScale = transform.localScale;

        SetTutorialType(pTutorialType);
    }

    public void UpdateIcon()
    {
        transform = targetObject.transform;
        transform.localScale = originalScale * floatingBehaviour.GetScaleFactor();
        Quaternion rotation = Quaternion.FromToRotation(transform.position, Camera.main.transform.position);
        rotation.eulerAngles += new Vector3(0, 90, 0);
        transform.rotation = rotation;
    }

    void SetTutorialType(TutorialType tutorialType)
    {
        Image image = targetObject.GetComponent<Image>();
        switch (tutorialType)
        {
            case TutorialType.TAB_BEETLE:
                image.sprite = Resources.Load<Sprite>(imgPath + "Beetle4");
                break;
            case TutorialType.TAB_FIREFLY:
                image.sprite = Resources.Load<Sprite>(imgPath + "Firefly4");
                break;
            case TutorialType.TAB_DRAGABLE:
                image.sprite = Resources.Load<Sprite>(imgPath + "Trash4");
                break;
            case TutorialType.TAB_LANTERN:
                image.sprite = Resources.Load<Sprite>(imgPath + "Lantern4");
                break;
            case TutorialType.SWIPE_TO_MOVE:
                image.sprite = Resources.Load<Sprite>(imgPath + "Swipe4");
                break;
        }
    }

    public void Destroy()
    {
        GameObject.Destroy(targetObject);
    }
}

public enum TutorialType
{
    TAB_BEETLE,
    TAB_DRAGABLE,
    TAB_FIREFLY,
    TAB_LANTERN,
    SWIPE_TO_MOVE
}
