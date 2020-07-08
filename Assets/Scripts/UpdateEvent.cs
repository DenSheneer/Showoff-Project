using System;
using UnityEngine;

public class UpdateEvent : MonoBehaviour
{
    public Action<int> myAction;
    private int myInteger = 0;
    private void Start()
    {
        Debug.Log("value before changes: " + myInteger);

        UpdateIntEvent(ref myInteger, 5, showUpdateNotification);
        Debug.Log("value after first change: " + myInteger);

        UpdateIntEvent(ref myInteger, -10, showUpdateNotification);
        Debug.Log("value after second change: " + myInteger);
    }

    public void UpdateIntEvent(ref int targetVariable, int addedValue, Action<int> pAction = null)
    {
        targetVariable += addedValue;
        pAction?.Invoke(targetVariable);
    }

    private void showUpdateNotification(int pValue)
    {
        Debug.Log("the value has been updated to: " + pValue);
    }
}
