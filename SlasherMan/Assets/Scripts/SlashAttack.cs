using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashAttack : Ability
{
    private int counter = 0;

    private bool targetHit = false;

    private void Start()
    {
        attackZone = GetComponent<BoxCollider>();
        attackZone.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        LivingThing l = other.gameObject.GetComponentInParent<LivingThing>();
        if (l != null)
        {
            l.takeDamage(damage);
            targetHit = true;
        }
    }

    private void FixedUpdate()
    {
        if(inUse)
        {
            if(counter++ > 1)
            {
                counter = 0;
                inUse = false;
                attackZone.enabled = false;

                if(targetHit)
                {
                    manager.registerSlash();
                    targetHit = false;
                }
            }
        }
    }

    private void Update()
    {
        if(canBeUsed())
        {
            if(Input.GetMouseButtonDown(0))
            {
                registerUse();
                inUse = true;
                attackZone.enabled = true;

                Vector3 target;

                if (tryFindTarget(out target))
                {
                    transform.parent.rotation = Quaternion.LookRotation(target - transform.parent.position);
                }
                else
                {
                    Debug.Log("Could not find floor");
                }

                //play anim
                Instantiate(anim, transform.position, transform.rotation);

                camShaker.shakeCamera(shakeDuration, shakeMagnitude);
            }
        }
    }
}
