using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackManager : MonoBehaviour
{
    private Collider attackTrigger;
    private MonsterAbility ability = null;

    // Start is called before the first frame update
    void Start()
    {
        attackTrigger = GetComponent<Collider>();
        attackTrigger.enabled = false;
        registerAbility();
    }

    public void registerAbility()
    {
        if(ability == null)
        {
            ability = transform.Find("Attacker").GetComponent<MonsterAbility>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (attackTrigger.enabled == false)
        {
            if (Time.realtimeSinceStartup - ability.lastCast > ability.monsterAttackCD)// + ability.timeBeforeAttack * 0.8f)
            {
                //Searching for a target entering attackzone
                attackTrigger.enabled = true;
            }
        }
    }

    public void deactivate()
    {
        ability.enabled = false;
        this.enabled = false;
    }

    public int getAbilityDamage() { return ability.damage; }

    public void changeAbilityDamage(int amount)
    {
        ability.damage = amount;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.isTrigger)
        {
            return;
        }

        if (other.attachedRigidbody != null)
        {
            if(other.attachedRigidbody.isKinematic)
            {
                return;
            }
        }

        if (other.transform.root.tag == "Player")
        {
            //Trigger the attack
            ability.lastCast = Time.realtimeSinceStartup;
            attackTrigger.enabled = false;
            //Debug.Log("disabling");
            ability.startAttack();
        }
    }
}
