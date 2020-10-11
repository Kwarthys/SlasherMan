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

            Vector3 target;
            if (tryFindTarget(out target))
            {
                transform.parent.rotation = Quaternion.LookRotation(target - transform.parent.position);
            }
        }

        if(inUse)
        {
            if(Time.realtimeSinceStartup - lastStart > duration)
            {
                controller.speed *= 1.5f;
                attackZone.enabled = false;
                particles.Stop();
                inUse = false;
            }

            Vector3 target;
            if (tryFindTarget(out target))
            {
                transform.parent.rotation = Quaternion.LookRotation(target - transform.parent.position);
            }

            return;
        }

        if(canBeUsed())
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                registerUse();
                manager.registerBlaze();

                //inUse = true;
                starting = true;

                lastStart = Time.realtimeSinceStartup;

                controller.speed /= 1.5f;

                playerAnimator.SetTrigger("Blaze");
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        dealDamage(other.gameObject.GetComponentInParent<LivingThing>());        
    }
}
