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

    private float targetRefreshTime = .05f;
    private float lastRefresh = -1;

    private Transform player;

    private Transform closestAlly;

    public MonsterAttackManager attackManager;

    public float seeDistance = 15;

    private MonsterDeathAnimationManager deathAnimationManager;

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

        player = GameObject.FindGameObjectWithTag("Player").transform;

        deathAnimationManager = GetComponent<MonsterDeathAnimationManager>();
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

            Vector3 target = player.position;

            if((target - transform.position).magnitude > seeDistance)
            {
                Vector2 r = Random.insideUnitCircle * seeDistance;
                target = transform.position;
                target.x += r.x;
                target.z += r.y;
            }

            if (closestAlly != null)
            {
                //Move away from it
                float distance = Vector3.Distance(closestAlly.position, transform.position);
                target -= (closestAlly.position - transform.position).normalized * friendAvoidDistance / distance;
                Debug.DrawLine(transform.position, target, Color.green);
            }

            agent.isStopped = !allowMovement;
            //agent.SetDestination(target);

            /*trying*/ //seems ok, but will have to monitor this
            NavMeshPath p = new NavMeshPath();
            NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, p);
            agent.SetPath(p);
        }
    }

    protected override void onDeath()
    {
        scoreManager.notifyKill(monsterKillScore);
        attackManager.deactivate();
        /*
        if (deathAnimation != null)
        {
            Instantiate(deathAnimation, transform.position, Quaternion.identity);
        }

        //ded
        //Destroy(gameObject);        
        */

        deathAnimationManager.startAnimation();
    }

    protected override void onTakeDamage()
    {
        //oof
    }
}
