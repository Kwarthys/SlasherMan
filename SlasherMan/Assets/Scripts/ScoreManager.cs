using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI text;

    public bool freeze = false;

    [Header("Animation")]
    public float minSize = 55;
    public float maxSize = 70;
    public float decrease = .1f;
    private float scaleAmount = 0;
    public float killIncrease = 0.1f;

    [SerializeField]
    private int score = 0;

    private void FixedUpdate()
    {
        //Debug.Log("SA " + scaleAmount);

        text.fontSize = Mathf.Lerp(minSize, maxSize, scaleAmount);
        scaleAmount = Mathf.Max(0, scaleAmount-decrease);
    }

    private void Start()
    {
        text.text = score.ToString();
    }

    public void notifyKill(int points)
    {
        if (freeze) return;

        score += points;
        text.text = score.ToString();

        scaleAmount = Mathf.Min(1, scaleAmount + killIncrease);
    }

    public void reinit()
    {
        score = 0;
        text.text = score.ToString();
        freeze = false;
    }
}
