using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreListElement : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nameText = null;

    [SerializeField]
    private TextMeshProUGUI scoreText = null;

    [SerializeField]
    private TextMeshProUGUI datePlayedText = null;

    public TextMeshProUGUI NameText { get => nameText; }
    public TextMeshProUGUI ScoreText { get => scoreText; }
    public TextMeshProUGUI DatePlayedText { get => datePlayedText; }



}
