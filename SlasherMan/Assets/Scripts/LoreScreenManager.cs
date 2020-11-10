using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoreScreenManager : MonoBehaviour
{
    public float startingFontSize = 30;
    public float endFontSize = 55;
    private float startTime = -1;

    public TextMeshProUGUI loreText;

    public float menuTime = 20;

    public GameManager gameManager;

    private void Start()
    {
        startTime = Time.realtimeSinceStartup;
    }

    // Update is called once per frame
    void Update()
    {
        float t = (Time.realtimeSinceStartup - startTime) / menuTime;

        if(t >= 1 || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            gameManager.startGame();
            gameObject.SetActive(false);
        }

        loreText.fontSize = Mathf.Lerp(startingFontSize, endFontSize, t);
    }
}
