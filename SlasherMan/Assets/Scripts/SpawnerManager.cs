﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public float spawnMinRange = 30;
    public float spawnMaxRange = 50;

    public float spawnRate = 1;
    private float lastSpawn = -10;

    public int wavesPerLevel = 30;
    private int waveCount = 0;

    public List<MonsterMeta> monsters = new List<MonsterMeta>();

    public int startCreditsPerSpawn = 10;
    public float coefIncrease = 1.15f;
    private float creditsPerSpawn = 0;
    private int waveCredits = 0;

    public int stageLevel = 1;

    private int mID = 0;

    private Transform player;

    public LayerMask floor;

    public int maxMonsterCount = 150;

    public bool wavesEnded = false;
    private bool waitingLoot = false;

    public GameManager manager;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        creditsPerSpawn = startCreditsPerSpawn;
    }

    public void resetForNext()
    {
        mID = 0;

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        lastSpawn = Time.realtimeSinceStartup - 10;

        waveCount = 0;
        wavesEnded = false;
        waitingLoot = false;
    }

    public void reinit()
    {
        resetForNext();

        creditsPerSpawn = startCreditsPerSpawn;
    }

    // Update is called once per frame
    void Update()
    {
        if (wavesEnded)
        {
            if (transform.childCount == 0)
            {
                if(!waitingLoot)
                {
                    manager.notifyLastMonsterKill();
                    waitingLoot = true;
                }
            }
        }
        else
        {
            if (Time.realtimeSinceStartup - lastSpawn > spawnRate && !wavesEnded)
            {
                lastSpawn = Time.realtimeSinceStartup;
                waveCredits = Mathf.RoundToInt(creditsPerSpawn);
                creditsPerSpawn = creditsPerSpawn * coefIncrease;
                //Spawn
                while(waveCredits > 0 && transform.childCount < maxMonsterCount)
                {
                    spawnAMonster();
                }

                waveCount++;
            }

            if(waveCount >= wavesPerLevel && !wavesEnded)
            {
                wavesEnded = true;
            }

        }
    }

    private void spawnAMonster()
    {
        Vector3 spawnPoint = getSpawnPoint();

        Monster m = Instantiate(getRandomMonsterPrefab(), spawnPoint, Quaternion.identity, transform).GetComponent<Monster>();
        m.transform.name = "Monster" + ++mID;

        //modify monster life and armor
        m.bonusArmor = stageLevel;
        m.changeBonusLife((int)Mathf.Pow(stageLevel,  1.10f));

        //modifyDamages
        m.attackManager.registerAbility();
        m.attackManager.changeAbilityDamage((int)(m.attackManager.getAbilityDamage() * Mathf.Pow(1.10f, stageLevel)));
    }

    private Vector3 getSpawnPoint()
    {
        Vector3 spawnPoint;

        do
        {
            spawnPoint = player.transform.position;
            Vector2 point = Random.insideUnitCircle.normalized;

            if (point.x == 0 && point.y == 0)
            {
                //In case the normalisation gives us (0,0), we force it to (1,0)
                point.x = 1;
            }

            point *= Random.Range(spawnMinRange, spawnMaxRange);

            spawnPoint.x += point.x;
            spawnPoint.z += point.y;
        } while (!Physics.Raycast(spawnPoint, Vector3.down, 3, floor));

        return spawnPoint;
    }

    private GameObject getRandomMonsterPrefab()
    {
        MonsterMeta m;
        int r;

        float rarityRandom = Random.value;

        do
        {
            r = Random.Range(0, monsters.Count);
            //Debug.Log(r + " / " + monsters.Count + " and " + rarityRandom);
            m = monsters[r];
        } while (m.monsterCost > waveCredits || rarityRandom < 1 - m.spawnChances);

        //Debug.Log("Spawned a " + m.monsterName + ". - " + waveCredits + "/" + creditsPerSpawn);

        waveCredits -= m.monsterCost;
        return m.monsterPrefab;
    }

    private void OnDrawGizmosSelected()
    {
        if(player != null)
        {
            Gizmos.DrawWireSphere(player.position, spawnMinRange);
            Gizmos.DrawWireSphere(player.position, spawnMaxRange);
        }
        else
        {
            Gizmos.DrawWireSphere(Vector3.zero, spawnMinRange);
            Gizmos.DrawWireSphere(Vector3.zero, spawnMaxRange);
        }
    }
}
