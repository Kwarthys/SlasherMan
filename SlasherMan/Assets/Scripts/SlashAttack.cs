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
                startSoundEffect();
                manager.releaseAttackBlock();
            }
        }
    }

    private void Update()
    {
        if(canBeUsed())
        {
            if(MyInputManager.Instance.slashKeyPressed())
            {
                registerUse();
                inUse = true;
                attackZone.enabled = true;

                Vector3 dir;

                if (tryFindAimDirection(out dir))
                {
                    transform.parent.rotation = Quaternion.LookRotation(dir);
                }

                //play anim
                Instantiate(anim, transform.position, transform.rotation);

                camShaker.shakeCamera(shakeDuration, shakeMagnitude);

                playerAnimator.SetTrigger("Slash");
            }
        }
    }
}
