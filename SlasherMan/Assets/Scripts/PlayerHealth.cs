using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingThing
{
    public Slider slider;

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

        slider.value = life * 1.0f / maxLife;
    }

    public override void init()
    {
    
    }
}
