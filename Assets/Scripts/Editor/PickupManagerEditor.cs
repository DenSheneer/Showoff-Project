using UnityEngine;
using UnityEditor;

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
            pickupManager.GetAllTapables();
        }

        base.OnInspectorGUI();
    }
}
