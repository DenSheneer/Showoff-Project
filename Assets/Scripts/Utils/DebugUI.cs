using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI tmp = null;

    float deltaTime = 0.0f;

    private void Start()
    {
        tmp.color = new Color(0, 1, 0);
    }

    private void Update()   // Author: Aras Pranckevicius (NeARAZ) 
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);

        tmp.text = text;
    }
}
