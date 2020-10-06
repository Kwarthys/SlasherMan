using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : LivingThing
{
    public float speed = 5;
    public float rotSpeed = 5;

    private bool allowMovement = true;

    public void setAllowMovement(bool state)
    {
        allowMovement = state;
    }


    private void Update()
    {
        //GetPlayer
        Vector3 target = GameObject.FindGameObjectWithTag("Player").transform.position;

        if(allowMovement)
        {
            transform.position += (target - transform.position).normalized * speed * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(target - transform.position), rotSpeed);
        }

        if (life <= 0)
        {
            if (deathAnimation != null)
            {
                Instantiate(deathAnimation, transform.position, Quaternion.identity);
            }

            //ded
            Destroy(gameObject);
        }
    }
}
