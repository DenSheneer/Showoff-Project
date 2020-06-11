using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEmissionHandler : MonoBehaviour
{
    [SerializeField]
    float EmissionMultiplier;

    void Update()
    {
        SkinnedMeshRenderer renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        Material mat = renderer.material;

        PlayerManager playerManager = GetComponent<PlayerManager>();

        float emission = playerManager.nrOfFlies * EmissionMultiplier;
        Color baseColor = Color.white;

        Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);

        mat.SetColor("_EmissionColor", finalColor);
    }
}
