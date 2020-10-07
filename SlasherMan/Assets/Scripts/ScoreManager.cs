using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI text;

    [SerializeField]
    private int score = 0;

    private void Start()
    {
        text.text = score.ToString();
    }

    public void notifyKill(int points)
    {
        score += points;
        text.text = score.ToString();
    }
}
