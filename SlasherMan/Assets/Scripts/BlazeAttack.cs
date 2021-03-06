﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlazeAttack : Ability
{
    private ParticleSystem particles;

    // Start is called before the first frame update
    protected override void onStart()
    {
        attackZone = GetComponent<BoxCollider>();
        attackZone.enabled = false;

        particles = transform.Find("Particle System").GetComponent<ParticleSystem>();
        particles.Stop();
    }

    protected override void prepareCast()
    {
        //controller.speed /= 1.5f;

        playerAnimator.SetTrigger("Blaze");

        newSteerToAim();
    }

    protected override void cast()
    {
        inUse = true;

        particles.Play();

        attackZone.enabled = true;

        camShaker.shakeCamera(shakeDuration, shakeMagnitude);
    }

    protected override void registerToManager()
    {
        manager.registerSpecial();
    }

    protected override bool inputPressed()
    {
        return MyInputManager.Instance.specialKeyPressed();
    }

    
    protected override void onUpdate()
    {
        if(inUse)
        {
            if(Time.realtimeSinceStartup - lastCast > duration)
            {
                //controller.speed *= 1.5f;
                attackZone.enabled = false;
                particles.Stop();
                inUse = false;
                manager.releaseAttackBlock();
            }

            steerToAim();

            return;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.isTrigger) return;
        dealDamage(other.gameObject.GetComponentInParent<LivingThing>());        
    }
}
