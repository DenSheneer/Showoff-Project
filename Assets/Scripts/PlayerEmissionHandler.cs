using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEmissionHandler : MonoBehaviour
{
    [SerializeField]
    float EmissionMultiplier = 1.0f;

    Material mat;
    Color baseColor = Color.white;
    int maxNrOfFlies;

    private void OnEnable()
    {
        GameObject bodyGO = GameObject.Find("kinker");
        SkinnedMeshRenderer renderer = bodyGO.GetComponent<SkinnedMeshRenderer>();
        mat = renderer.material;

        PlayerManager temp = GetComponent<PlayerManager>();
        temp.OnFireflyChange += UpdateGFX;
        maxNrOfFlies = temp.MaxNrOfFlies;
    }

    void UpdateGFX(int nrOfFireflies)
    {
        float emission;

        if (nrOfFireflies < maxNrOfFlies)
            emission = nrOfFireflies * EmissionMultiplier;
        else emission = maxNrOfFlies * EmissionMultiplier;

        Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);

        mat.SetColor("_EmissionColor", finalColor);
    }
}
