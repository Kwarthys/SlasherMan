using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    [Header("LoreScreen")]
    public GameObject loreScreen;

    [Header("EndScreen")]
    public GameObject endScreen;
    public Image endScreenBackGound;
    public GameObject abilityStatsContainer;
    public GameObject abilityTextPrefab;
    public TextMeshProUGUI defeatText;

    private bool playerDead = false;
    public float fadeInTime = 2;
    private float startFadeInTime = -2;

    public float maxAlphaValue = 0.8f;
    public float minDefeatTextSize = 30;
    public float maxDefeatTextSize = 70;

    [Header("EndOfLevel")]
    public int stageNumber = 1;
    public GameObject levelEndScreen;
    public InventoryDisplayer inventoryDisplayer;
    public InventoryManager inventoryManager;

    [Header("RestartTheGame")]
    public SpawnerManager spawnManager;
    public AttackManager attackManager;
    public ScoreManager scoreManager;
    public PlayerHealth playerHealth;
    public PlayerController playerController;
    public FurnitureSpawnerManager furnitureManager;

    private void Start()
    {
        spawnManager.gameObject.SetActive(false);
        furnitureManager.gameObject.SetActive(false);
        loreScreen.SetActive(true);
        stageNumber = 1;
    }

    public void startGame()
    {
        spawnManager.gameObject.SetActive(true);
        furnitureManager.gameObject.SetActive(true);
        retryClick();
    }

    public void initialise()
    {
        endScreenBackGound.color = new Color(endScreenBackGound.color.r, endScreenBackGound.color.g, endScreenBackGound.color.b, 0);
        defeatText.fontSize = minDefeatTextSize;
        endScreen.SetActive(false);
        levelEndScreen.SetActive(false);
        playerDead = false;

        foreach(Transform child in abilityStatsContainer.transform)
        {
            Destroy(child.gameObject);
        }

        attackManager.masterAttackBlock = false;
    }

    public void notifyPlayerDead()
    {
        if(!playerDead)
        {
            endScreen.SetActive(true);
            startFadeInTime = Time.realtimeSinceStartup;
            playerDead = true;
            buildAbilitiesStats();
            playerController.canMove = false;
            attackManager.masterAttackBlock = true;
        }
    }

    private void Update()
    {
        if(playerDead)
        {
            float t = (Time.realtimeSinceStartup - startFadeInTime) / fadeInTime;
            float alpha = Mathf.Min(maxAlphaValue, t);
            endScreenBackGound.color = new Color(endScreenBackGound.color.r, endScreenBackGound.color.g, endScreenBackGound.color.b, alpha);

            if (t > 1) t = 1;
            float size = Mathf.Lerp(minDefeatTextSize, maxDefeatTextSize, t);
            defeatText.fontSize = size;

            scoreManager.freeze = true;
        }
    }

    private void buildAbilitiesStats()
    {
        foreach(Ability a in attackManager.getAbilities())
        {
            string text = a.abilityName + ":\n";
            string damage = "Damage: " + a.totalDamage;

            while(damage.Length < 30)
            {
                damage += "_";
            }

            damage += "Kills: " + a.totalKills;

            text += damage;

            TextMeshProUGUI textElement = Instantiate(abilityTextPrefab, abilityStatsContainer.transform).GetComponent<TextMeshProUGUI>();
            textElement.text = text;
        }
    }

    public void startNextStage()
    {
        //Start new level
        inventoryDisplayer.removeLootIfLeft();
        attackManager.init();
        spawnManager.resetForNext();
        furnitureManager.reinit();
        playerHealth.init();
        initialise();

        playerController.canMove = true;
    }

    public void notifyLastMonsterKill()
    {
        if (playerDead) return;
        //GG
        levelEndScreen.SetActive(true);
        inventoryDisplayer.refreshSlots();
        //Loot
        //Debug.Log("Generate a Loot");
        inventoryDisplayer.generateALoot(stageNumber++);

        playerController.canMove = false;
        attackManager.masterAttackBlock = true;
    }

    public void retryClick()
    {
        //Debug.Log("retry");
        foreach (Ability a in attackManager.getAbilities())
        {
            if(a!=null)
            {
                a.totalDamage = 0;
                a.totalKills = 0;
            }
        }

        attackManager.init();
        spawnManager.reinit();
        scoreManager.reinit();
        furnitureManager.reinit();
        playerHealth.init();
        playerController.canMove = true;

        initialise();
    }

    public void quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
