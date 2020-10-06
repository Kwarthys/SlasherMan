using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : LivingThing
{
    void Update()
    {
        if (life <= 0)
        {
            if (deathAnimation != null)
            {
                Instantiate(deathAnimation, transform.position, Quaternion.identity);
            }

            //ded
            transform.root.position = Vector3.zero;
            life = maxLife;
        }
    }
}
