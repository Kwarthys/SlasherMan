using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ItemSpawnerManager : MonoBehaviour
{
    public GameObject deskPrefab;
    public float gridUnitSize = 2;

    public float mapSize = 50;

    // Start is called before the first frame update
    void Start()
    {
        for(float j = -mapSize; j < mapSize; j+=gridUnitSize)
        {
            for (float i = -mapSize; i < mapSize; i += gridUnitSize)
            {
                if(Random.value > 0.3f)
                {
                    Instantiate(deskPrefab, new Vector3(i, 0, j), Random.value > 0.5f ? Quaternion.identity : Quaternion.LookRotation(Vector3.back), transform);
                }
            }
        }        
    }

    public void reinit()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        Start();
    }
}
