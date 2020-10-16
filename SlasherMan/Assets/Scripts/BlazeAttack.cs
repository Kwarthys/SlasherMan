using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlazeAttack : Ability
{
    public PlayerController controller;

    public float duration = 60;
    private float lastStart = -1;

    public float initTime = .5f;
    private bool starting = false;

    private ParticleSystem particles;

    private Vector3 dirBLAZE;

    // Start is called before the first frame update
    void Start()
    {
        attackZone = GetComponent<BoxCollider>();
        attackZone.enabled = false;

        particles = transform.Find("Particle System").GetComponent<ParticleSystem>();
        particles.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if(starting)
        {
            if(Time.realtimeSinceStartup - lastStart > initTime)
            {
                inUse = true;
                starting = false;

                //Actually launching the spell

                particles.Play();

                attackZone.enabled = true;

                camShaker.shakeCamera(shakeDuration, shakeMagnitude);
            }

            steerToAim();
        }

        if(inUse)
        {
            if(Time.realtimeSinceStartup - lastStart > duration)
            {
                controller.speed *= 1.5f;
                attackZone.enabled = false;
                particles.Stop();
                inUse = false;
                manager.releaseAttackBlock();
            }

            steerToAim();

            return;
        }

        if(canBeUsed())
        {
            if(MyInputManager.Instance.blazeKeyPressed())
            {
                registerUse();
                startSoundEffect();
                manager.registerBlaze();

                //inUse = true;
                starting = true;

                lastStart = Time.realtimeSinceStartup;

                controller.speed /= 1.5f;

                playerAnimator.SetTrigger("Blaze");

                if(MyInputManager.Instance.tryGetAimDirection(out dirBLAZE))
                {
                    //yee we have an aim
                }
                else
                {
                    dirBLAZE = transform.forward;
                }
            }
        }
    }

    private void steerToAim()
    {
        if (tryFindAimDirection(out Vector3 dir))
        {
            dirBLAZE = dir;
        }

        transform.parent.rotation = Quaternion.LookRotation(dirBLAZE);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.isTrigger) return;
        dealDamage(other.gameObject.GetComponentInParent<LivingThing>());        
    }
}
