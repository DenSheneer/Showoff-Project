using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIPassword : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI pinCodeText = null;

    private string currentCode = "";

    [SerializeField]
    private string passpin = "1234";

    private void OnEnable()
    {
        pinCodeText.text = "";
        currentCode = "";
    }

    public void NumberPressed(int number)
    {
        if (currentCode.Length >= passpin.Length)
        {
            Debug.Log("code to long");
            return;
        }

        currentCode += number.ToString();
        pinCodeText.text = currentCode;
    }

    public void DeleteNumber()
    {
        if (currentCode.Length == 0)
        {
            Debug.Log("no code pressed");
            return;
        }

        currentCode = currentCode.Remove(currentCode.Length - 1);
        pinCodeText.text = currentCode;
    }

    public void TryPin()
    {
        if (currentCode == passpin)
        {
            Debug.Log("correct pass");
        }
        else
        {
            Debug.Log("wrong pass");
        }
    }
}
