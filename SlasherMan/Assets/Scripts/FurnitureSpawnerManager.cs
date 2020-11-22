using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FurnitureSpawnerManager : MonoBehaviour
{
    public List<SceneItemMeta> objects = new List<SceneItemMeta>();

    public Vector2 gridUnitSize = new Vector2(6.5f,6.5f);

    public float mapSize = 50;

    public float density = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        for(float j = -mapSize + gridUnitSize.y/2; j < mapSize - gridUnitSize.y/2; j+=gridUnitSize.y)
        {
            for (float i = -mapSize + gridUnitSize.x/2; i < mapSize - gridUnitSize.x/2; i += gridUnitSize.x)
            {
                if(Random.value > 1-density)
                {
                    GameObject prefab = getRandomObject(out bool canMoveInCell);

                    float x = i;// + Random.value * gridUnitSize/2;
                    float y = j;// + Random.value * gridUnitSize/2;

                    if(canMoveInCell)
                    {
                        x += Random.value * gridUnitSize.x / 1.8f;
                        y += Random.value * gridUnitSize.y / 1.8f;
                    }

                    Instantiate(prefab, new Vector3(x, 0, y), Random.value > 0.5f ? Quaternion.identity : Quaternion.LookRotation(Vector3.back), transform);
                }
            }
        }

        transform.Rotate(0, 45 * (Random.value > 0.5f ? 1 : -1), 0);
    }

    private GameObject getRandomObject(out bool canMoveInCell)
    {
        float rarity = Random.value;

        SceneItemMeta item;

        do
        {
            item = objects[Random.Range(0, objects.Count)];
        } while (item.probability <= rarity);

        canMoveInCell = item.canMoveInCell;
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
