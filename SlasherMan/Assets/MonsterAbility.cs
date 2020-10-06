using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAbility : MonoBehaviour
{
    public float monsterAttackCD = 1;
    public float lastCast = -1;

    public float timeBeforeAttack = 1;
    private float timeOfAttackStart = -1;
    private bool attacking = false;

    public GameObject attackAnim;

    public int damage = 20;

    private Collider attackZone;

    private Monster controller;

    private int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        attackZone = GetComponent<Collider>();
        attackZone.enabled = false;

        controller = transform.root.GetComponent<Monster>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Trigger the attack
        //Debug.Log("detected " + other.transform.root.name);

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
