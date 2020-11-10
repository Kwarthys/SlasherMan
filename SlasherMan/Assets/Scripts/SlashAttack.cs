using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashAttack : Ability
{
    private int counter = 0;

    private bool targetHit = false;

    protected override void onStart()
    {
        attackZone = GetComponent<BoxCollider>();
        attackZone.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;
        LivingThing l = other.gameObject.GetComponentInParent<LivingThing>();
        if (l != null)
        {
            dealDamage(l);
            targetHit = true;
        }
    }

    private void FixedUpdate()
    {
        if(inUse)
        {
            steerToAim();

            if(counter++ > 1)
            {
                counter = 0;
                inUse = false;
                attackZone.enabled = false;

                if (targetHit)
                {
                    manager.registerSlash();
                    targetHit = false;

                    //startSoundEffect();
                }
                // else
                //{
                //     startSoundEffectNoHit();
                // }
                //startSoundEffect();
                manager.releaseAttackBlock();
            }
        }
    }

    protected override void prepareCast()
    {
        if (tryFindAimDirection(out Vector3 dir))
        {
            transform.parent.rotation = Quaternion.LookRotation(dir);
        }

        //play anim
        Instantiate(anim, transform.position, transform.rotation);

        camShaker.shakeCamera(shakeDuration, shakeMagnitude);

        playerAnimator.SetTrigger("Slash");

        newSteerToAim();

        controller.speed /= 1.2f; 
    }

    protected override void cast()
    {
        attackZone.enabled = true;
        inUse = true;

        controller.speed *= 1.2f;
    }

    protected override void registerToManager()
    {
        //manager.registerSlash();
    }

    protected override bool inputPressed()
    {
        return MyInputManager.Instance.slashKeyPressed();
    }
}
