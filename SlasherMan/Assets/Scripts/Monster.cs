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

    private static ScoreManager scoreManager;
    public int monsterKillScore = 1;

    private NavMeshAgent agent;

    private float targetRefreshTime = .3f;
    private float lastRefresh = -1;

    private Transform closestAlly;

    public void setAllowMovement(bool state)
    {
        allowMovement = state;
    }

    public override void init()
    {
        if(scoreManager == null)
        {
            scoreManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<ScoreManager>();
        }
        agent = GetComponent<NavMeshAgent>();
    }

    private void FixedUpdate()
    {
        if (Time.realtimeSinceStartup - lastRefresh > targetRefreshTime)
        {
            lastRefresh = Time.realtimeSinceStartup;

            closestAlly = null;
            foreach (Collider other in Physics.OverlapSphere(transform.position, friendAvoidDistance, monsterLayer, QueryTriggerInteraction.Ignore))
            {
                if (other.transform.parent != transform)
                {
                    //Debug.Log(transform.name + " detected " + other.transform.name + " with parent " + other.transform.parent.name);
                    if (closestAlly == null)
                    {
                        closestAlly = other.transform.parent;
                    }
                    else if ((closestAlly.position - transform.position).sqrMagnitude > (other.transform.parent.position - transform.position).sqrMagnitude)
                    {
                        closestAlly = other.transform.parent;
                    }
                }
            }

            //GetPlayer
            Vector3 target = GameObject.FindGameObjectWithTag("Player").transform.position;
            Debug.DrawLine(transform.position, target, Color.red);

            if (closestAlly != null)
            {
                //Move away from it
                float distance = Vector3.Distance(closestAlly.position, transform.position);
                target -= (closestAlly.position - transform.position).normalized * friendAvoidDistance / distance;
                //Debug.Log(distance + " " + (closestAlly.position - transform.position).normalized * friendAvoidDistance / distance);
                Debug.DrawLine(transform.position, target, Color.green);
            }

            agent.isStopped = !allowMovement;
            //transform.position += (target - transform.position).normalized * speed * Time.deltaTime;
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(target - transform.position), rotSpeed);
            agent.SetDestination(target);
        }
    }

    protected override void onDeath()
    {
        if (deathAnimation != null)
        {
            Instantiate(deathAnimation, transform.position, Quaternion.identity);
        }

        //ded
        scoreManager.notifyKill(monsterKillScore);
        Destroy(gameObject);        
    }

    protected override void onTakeDamage()
    {
        //oof
    }
}
