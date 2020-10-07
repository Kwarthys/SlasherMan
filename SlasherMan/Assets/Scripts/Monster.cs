using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : LivingThing
{
    public float speed = 5;
    public float rotSpeed = 5;

    private bool allowMovement = true;

    public LayerMask monsterLayer;

    public float friendAvoidDistance = 3;

    public void setAllowMovement(bool state)
    {
        allowMovement = state;
    }


    private void Update()
    {
        Transform closestAlly = null;
        foreach(Collider other in Physics.OverlapSphere(transform.position, friendAvoidDistance, monsterLayer))
        {
            if(other.transform.root != transform.root)
            {
                if (closestAlly == null)
                {
                    closestAlly = other.transform.root;
                }
                else if (Vector3.Distance(closestAlly.position, transform.position) > Vector3.Distance(other.transform.root.position, transform.position))
                {
                    closestAlly = other.transform.root;
                }
            }            
        }

        //GetPlayer
        Vector3 target = GameObject.FindGameObjectWithTag("Player").transform.position;
        Debug.DrawLine(transform.position, target, Color.red);

        if(closestAlly != null)
        {
            //Move away from it
            float distance = Vector3.Distance(closestAlly.position, transform.position);
            target -= (closestAlly.position - transform.position).normalized * friendAvoidDistance / distance;
            Debug.DrawLine(transform.position, target, Color.green);
        }


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
