using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ItemSpawnerManager : MonoBehaviour
{
    public List<SceneItemMeta> objects = new List<SceneItemMeta>();

    public float gridUnitSize = 2;

    public float mapSize = 50;

    // Start is called before the first frame update
    void Start()
    {
        for(float j = -mapSize + gridUnitSize/2; j < mapSize - gridUnitSize/2; j+=gridUnitSize)
        {
            for (float i = -mapSize + gridUnitSize/2; i < mapSize - gridUnitSize/2; i += gridUnitSize)
            {
                if(Random.value > 0.2f)
                {
                    GameObject prefab = getRandomObject();

                    float x = i;// + Random.value * gridUnitSize/2;
                    float y = j;// + Random.value * gridUnitSize/2;

                    Instantiate(prefab, new Vector3(x, 0, y), Random.value > 0.5f ? Quaternion.identity : Quaternion.LookRotation(Vector3.back), transform);
                }
            }
        }

        transform.Rotate(0, -45, 0);
    }

    private GameObject getRandomObject()
    {
        float rarity = Random.value;

        SceneItemMeta item;

        do
        {
            item = objects[Random.Range(0, objects.Count)];
        } while (item.probability <= rarity);

        return item.prefab;
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
