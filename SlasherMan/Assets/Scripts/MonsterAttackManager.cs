using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackManager : MonoBehaviour
{
    private Collider attackTrigger;

    private MonsterAbility ability;

    // Start is called before the first frame update
    void Start()
    {
        attackTrigger = GetComponent<Collider>();
        attackTrigger.enabled = false;

        ability = transform.Find("Attacker").GetComponent<MonsterAbility>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.realtimeSinceStartup - ability.lastCast > ability.monsterAttackCD)
        {
            //Searching for a target entering attackzone
            attackTrigger.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.root.tag == "Player")
        {
            //Trigger the attack
            attackTrigger.enabled = false;
            ability.lastCast = Time.realtimeSinceStartup;
            ability.startAttack();
        }
    }
}
