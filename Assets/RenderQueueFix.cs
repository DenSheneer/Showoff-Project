using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderQueueFix : MonoBehaviour
{
    public Renderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        renderer.materials[1].renderQueue = 2900;
    }
}
