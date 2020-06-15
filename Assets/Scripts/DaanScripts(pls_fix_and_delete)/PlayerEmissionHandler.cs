using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEmissionHandler : MonoBehaviour
{
    [SerializeField]
    float EmissionMultiplier = 1.0f;

    PlayerManager playerManager;
    Material mat;
    Color baseColor = Color.white;

    private void Start()
    {
        GameObject bodyGO = GameObject.Find("kinker");
        SkinnedMeshRenderer renderer = bodyGO.GetComponent<SkinnedMeshRenderer>();
        mat = renderer.material;

        playerManager = GetComponent<PlayerManager>();
    }

    void Update()
    {
        float emission = playerManager.NrOfFlies * EmissionMultiplier;

        Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);

        mat.SetColor("_EmissionColor", finalColor);
    }
}
