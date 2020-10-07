using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : LivingThing
{
    public float speed = 5;
    public float rotSpeed = 5;

    private bool allowMovement = true;

    public LayerMask monsterLayer;

    public float friendAvoidDistance = 3;

    private ScoreManager scoreManager;

    private NavMeshAgent agent;

    private int targetRefreshTime = 20;
    private int refreshCounter = 0;

    public void setAllowMovement(bool state)
    {
        allowMovement = state;
    }

    public override void init()
    {
        scoreManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<ScoreManager>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        Transform closestAlly = null;
        foreach(Collider other in Physics.OverlapSphere(transform.position, friendAvoidDistance, monsterLayer, QueryTriggerInteraction.Ignore))
        {
            if (other.transform.parent != transform)
            {
                //Debug.Log(transform.name + " detected " + other.transform.name + " with parent " + other.transform.parent.name);
                if (closestAlly == null)
                {
                    closestAlly = other.transform.parent;
                }
                else if (Vector3.Distance(closestAlly.position, transform.position) > Vector3.Distance(other.transform.parent.position, transform.position))
                {
                    closestAlly = other.transform.parent;
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
            //Debug.Log(distance + " " + (closestAlly.position - transform.position).normalized * friendAvoidDistance / distance);
            Debug.DrawLine(transform.position, target, Color.green);
        }

        agent.isStopped = !allowMovement;

        if(refreshCounter > targetRefreshTime)
        {
            //transform.position += (target - transform.position).normalized * speed * Time.deltaTime;
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(target - transform.position), rotSpeed);
            agent.SetDestination(target);
            refreshCounter = 0;
        }

        if (life <= 0)
        {
            if (deathAnimation != null)
            {
                Instantiate(deathAnimation, transform.position, Quaternion.identity);
            }

            //ded
            scoreManager.notifyKill(1);
            Destroy(gameObject);
        }

        refreshCounter++;
    }
}
