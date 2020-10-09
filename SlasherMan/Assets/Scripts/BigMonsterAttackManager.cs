using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigMonsterAttackManager : MonoBehaviour
{
    public GameObject singleAnimPrefab;

    public float size = 6;
    public int number = 7;

    // Start is called before the first frame update
    void Start()
    {
        //float spaceBetween = number / size;
        float spaceBetween = size / number;

        for (int i = 0; i < number; ++i)
        {
            Vector3 pos = transform.position + transform.forward * ((i+1)*spaceBetween);
            Instantiate(singleAnimPrefab, pos, Quaternion.identity, transform);
        }
    }
}
