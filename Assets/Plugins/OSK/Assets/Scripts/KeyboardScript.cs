//  By Denis from FiveOK

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyboardScript : MonoBehaviour
{
    public TextMeshProUGUI TargetTextField; 

    public GameObject EngLayoutSml, EngLayoutBig, SymbLayout;

    public void alphabetFunction(string alphabet)
    {
        TargetTextField.text=TargetTextField.text + alphabet;
    }

    public void BackSpace()
    {

        if(TargetTextField.text.Length>0) TargetTextField.text= TargetTextField.text.Remove(TargetTextField.text.Length-1);

    }

    public void CloseAllLayouts()
    {
        EngLayoutSml.SetActive(false);
        EngLayoutBig.SetActive(false);
        SymbLayout.SetActive(false);

    }

    public void ShowLayout(GameObject SetLayout)
    {

        CloseAllLayouts();
        SetLayout.SetActive(true);

    }

}
