using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ResetButton : MonoBehaviour
{
    Button button;
    private void OnEnable()
    {
        button = GetComponent<Button>();
    }

    public void AddButtonFunction(UnityAction action)
    {
        button.onClick.AddListener(action);
    }
}
