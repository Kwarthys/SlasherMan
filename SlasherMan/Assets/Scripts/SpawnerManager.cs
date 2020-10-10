using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public float spawnRange = 30;

    public float spawnRate = 1;
    private float lastSpawn = -1;

    //public GameObject monsterPrefab;

    public List<MonsterMeta> monsters = new List<MonsterMeta>();

    //public LayerMask monsterLayer;
    //public GameObject monsterDeathAnimation;

    public int creditsPerSpawn = 10;
    private int waveCredits = 0;

    private int mID = 0;

    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.realtimeSinceStartup - lastSpawn > spawnRate)
        {
            lastSpawn = Time.realtimeSinceStartup;
            waveCredits = creditsPerSpawn;
            //Spawn
            while(waveCredits > 0)
            {
                spawnAMonster();
            }
        }
    }

    private void spawnAMonster()
    {
        Vector3 spawnPoint = player.transform.position;
        Vector2 point = Random.insideUnitCircle.normalized;
        if(point.x == 0 && point.y == 0)
        {
            point.x = 1;
        }

        point *= spawnRange;

        spawnPoint.x += point.x;
        spawnPoint.z += point.y;

        Monster m = Instantiate(getRandomMonsterPrefab(), spawnPoint, Quaternion.identity, transform).GetComponent<Monster>();
        //m.monsterLayer = monsterLayer;
        //m.deathAnimation = monsterDeathAnimation;
        m.transform.name = "Monster" + ++mID;
    }

    private GameObject getRandomMonsterPrefab()
    {
        MonsterMeta m;
        int r;

        float rarityRandom = Random.value;

        do
        {
            r = Random.Range(0, monsters.Count);
            Debug.Log(r + " / " + monsters.Count + " and " + rarityRandom);
            m = monsters[r];
        } while (m.monsterCost > waveCredits || rarityRandom < 1 - m.spawnChances);

        Debug.Log("Spawned a " + m.monsterName + ". - " + waveCredits + "/" + creditsPerSpawn);

        waveCredits -= m.monsterCost;
        return m.monsterPrefab;
    }
}
