using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public float spawnRange = 30;

    public float spawnRate = 1;
    private float lastSpawn = -1;

    public float spawnAmount = 1;

    public GameObject monsterPrefab;

    public LayerMask monsterLayer;
    public GameObject monsterDeathAnimation;

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
            //Spawn
            for(int i = 0; i < spawnAmount; ++i)
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

        Monster m = Instantiate(monsterPrefab, spawnPoint, Quaternion.identity, transform).GetComponent<Monster>();
        m.monsterLayer = monsterLayer;
        m.deathAnimation = monsterDeathAnimation;
        m.transform.name = "Monster" + ++mID;
    }
}
