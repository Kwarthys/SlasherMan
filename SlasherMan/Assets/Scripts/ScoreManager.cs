using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI text;

    public bool freeze = false;

    [SerializeField]
    private int score = 0;

    private void Start()
    {
        text.text = score.ToString();
    }

    public void notifyKill(int points)
    {
        if (freeze) return;

        score += points;
        text.text = score.ToString();
    }

    public void reinit()
    {
        score = 0;
        text.text = score.ToString();
        freeze = false;
    }
}
