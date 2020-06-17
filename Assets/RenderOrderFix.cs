using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderOrderFix : MonoBehaviour
{
    Renderer GFX_Renderer;
    private void OnEnable()
    {
        GFX_Renderer = GetComponentInChildren<Renderer>();

        GFX_Renderer.materials[1].renderQueue = 2050;
    }
}
