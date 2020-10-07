using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAbility : MonoBehaviour
{
    public Vector3 attackZoneSize;

    public float monsterAttackCD = 1;
    public float lastCast = -1;

    public float timeBeforeAttack = 1;
    private float timeOfAttackStart = -1;
    private bool attacking = false;

    public GameObject preAttackAnim;
    private HighLightResizer preAttackAnimInstanciated;

    public GameObject attackAnim;

    public int damage = 20;

    private BoxCollider attackZone;

    private Monster controller;

    private int counter = 0;

    private bool freezeDamage = false;

    // Start is called before the first frame update
    void Start()
    {
        attackZone = GetComponent<BoxCollider>();
        attackZone.enabled = false;

        controller = transform.parent.GetComponent<Monster>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Trigger the attack
        //Debug.Log("detected " + other.transform.root.name);

        if (freezeDamage) return;

        LivingThing l = other.GetComponentInParent<LivingThing>();
        if(l!=null)
        {
            l.takeDamage(damage);
            //Debug.Log("hit " + other.transform.root.name);
        }
    }

    private void Update()
    { 
        if(attacking)
        {
            if(Time.realtimeSinceStartup - timeOfAttackStart > timeBeforeAttack)
            {
                attacking = false;
                attackZone.enabled = true;

                //AttackAnim
                //Debug.Log("Attacking");
                Instantiate(attackAnim, transform.position, transform.rotation);

                Destroy(preAttackAnimInstanciated.gameObject);
            }
        }
    }

    public void startAttack()
    {
        attacking = true;
        timeOfAttackStart = Time.realtimeSinceStartup;

        controller.setAllowMovement(false);

        //StartAttackAnim
        //Debug.Log("StartAttack");
        freezeDamage = true;
        attackZone.enabled = true;
        preAttackAnimInstanciated = Instantiate(preAttackAnim, attackZone.bounds.center, transform.rotation, transform).GetComponent<HighLightResizer>();
        preAttackAnimInstanciated.resize(attackZone.size);
        freezeDamage = false;
        attackZone.enabled = false;
    }

    private void FixedUpdate()
    {
        if(attackZone.enabled == true)
        {
            if(counter++ > 3)
            {
                attackZone.enabled = false;
                counter = 0;
                controller.setAllowMovement(true);
            }
        }
    }
}
