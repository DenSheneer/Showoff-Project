using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI tmp = null;

    Image[] fireflyImages = new Image[3];

    float deltaTime = 0.0f;

    private void Start()
    {
        tmp.color = new Color(0, 1, 0);
        GetComponent<Canvas>().worldCamera = Camera.main;

        GameObject fireflyElement = GameObject.Find("Fireflies_Background");

        int i = 0;
        foreach (Image image in fireflyElement.GetComponentsInChildren<Image>())
        {
            if (image.name != fireflyElement.name)
            {
                fireflyImages[i] = image;
                image.color = new Color(1, 1, 1, .25f);
                i++;
            }
        }
    }

    public void UpdateUI(int newNrOfFireflies)
    {
        foreach (Image image in fireflyImages)
            image.color = new Color(1, 1, 1, .25f);

        int toBeAdded = 0;

        if (newNrOfFireflies <= fireflyImages.Length)
            toBeAdded = newNrOfFireflies;
        else
            toBeAdded = fireflyImages.Length;

        for (int i = 0; i < toBeAdded; i++)
            fireflyImages[i].color = new Color(1, 1, 1, 1);
    }

    private void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);

        tmp.text = text;
    }
}
