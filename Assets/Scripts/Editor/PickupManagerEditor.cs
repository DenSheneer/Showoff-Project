using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Security.Cryptography;

[CustomEditor(typeof(PickupManager))]
public class PickupManagerEditor : Editor
{
    private GameObject[] allGameObjects = null;
    private PickupManager pickupManager = null;

    public void OnEnable()
    {
        if (target is PickupManager)
            pickupManager = target as PickupManager;
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Get all tapbles"))
        {
            allGameObjects = GameObject.FindObjectsOfType<GameObject>();

            foreach (GameObject gameObject in allGameObjects)
            {
                TapAble tapAble = gameObject.GetComponent<TapAble>();

                if (tapAble != null)
                    pickupManager.AddTapble(tapAble);
            }

        }

        base.OnInspectorGUI();
    }
}
