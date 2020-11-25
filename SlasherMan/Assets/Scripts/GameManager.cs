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
    private Dictionary<string, int> abilityTotalDamages = new Dictionary<string, int>();
    private Dictionary<string, int> abilityTotalKills = new Dictionary<string, int>();
    public AudioManager audioManager;

    private bool playerDead = false;
    private bool endscreenanim = false;
    public float fadeInTime = 2;
    private float startFadeInTime = -2;

    public float maxAlphaValue = 0.8f;
    public float minDefeatTextSize = 30;
    public float maxDefeatTextSize = 70;

    [Header("EndOfLevel")]
    public MusicManager musicManager;
    public int stageNumber = 1;
    public GameObject levelEndScreen;
    public InventoryDisplayer inventoryDisplayer;
    public InventoryManager inventoryManager;
    public GameObject nextStageButton;

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
        musicManager.transiToMusic();
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

        musicManager.transiToMusic();
        audioManager.ambianceVolumeCoef = 1;

        foreach(Transform child in abilityStatsContainer.transform)
        {
            Destroy(child.gameObject);
        }

        attackManager.masterAttackBlock = false;

        playerHealth.hasTie = false;

        nextStageButton.SetActive(true);
    }

    public void notifyPlayerDead()
    {
        //Loot the tie
        levelEndScreen.SetActive(true);
        inventoryDisplayer.refreshSlots();
        inventoryDisplayer.generateTieLoot();
        playerDead = true;

        nextStageButton.SetActive(false);

        playerController.canMove = false;
        attackManager.masterAttackBlock = true;
    }

    private void showDeathScreen()
    {
        musicManager.transiToBass();
        endScreen.SetActive(true);
        startFadeInTime = Time.realtimeSinceStartup;
        registerAndResetWeaponsStats();
        buildAbilitiesStats();

        scoreManager.freeze = true;
    }

    private void Update()
    {
        if(playerHealth.hasTie && !endscreenanim)
        {
            showDeathScreen();
            endscreenanim = true;
        }

        if(endscreenanim)
        {
            float t = (Time.realtimeSinceStartup - startFadeInTime) / fadeInTime;
            float alpha = Mathf.Min(maxAlphaValue, t);
            endScreenBackGound.color = new Color(endScreenBackGound.color.r, endScreenBackGound.color.g, endScreenBackGound.color.b, alpha);

            if (t > 1) t = 1;
            audioManager.ambianceVolumeCoef = 1 - t;
            float size = Mathf.Lerp(minDefeatTextSize, maxDefeatTextSize, t);
            defeatText.fontSize = size;
        }
    }

    private void buildAbilitiesStats()
    {
        foreach(string abilityName in abilityTotalDamages.Keys)
        {
            string text = abilityName + ":\n";
            string damage = "Damage: " + abilityTotalDamages[abilityName];

            string kills = "Kills: " + abilityTotalKills[abilityName];

            while (damage.Length + kills.Length < 30)
            {
                damage += "_";
            }

            damage += kills;

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
        playerHealth.replace();
        initialise();

        musicManager.transiToMusic();

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

        spawnManager.stageLevel = stageNumber;

        musicManager.transiToBass();

        registerAndResetWeaponsStats();
    }

    private void registerAndResetWeaponsStats()
    {
        foreach (Ability a in attackManager.getAbilities())
        {
            if(a!=null)
            {
                if (!abilityTotalDamages.ContainsKey(a.abilityName))
                {
                    abilityTotalDamages.Add(a.abilityName, 0);
                    abilityTotalKills.Add(a.abilityName, 0);
                }

                abilityTotalDamages[a.abilityName] += a.totalDamage;
                abilityTotalKills[a.abilityName] += a.totalKills;

                a.totalKills = 0;
                a.totalDamage = 0;
            }
        }
    }

    public void retryClick()
    {
        endscreenanim = false;

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

        inventoryManager.reinit();

        stageNumber = 1;

        initialise();
    }

    public void quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
