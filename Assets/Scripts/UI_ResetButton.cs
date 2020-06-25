using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UI_ResetButton : MonoBehaviour
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
