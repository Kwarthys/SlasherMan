using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlazeAttack : Ability
{
    public PlayerController controller;

    public float duration = 60;
    private float lastStart = -1;

    public float dotTime = .5f;
    private float lastDotTime = -1;

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
        if(inUse)
        {
            if(Time.realtimeSinceStartup - lastStart > duration)
            {
                controller.speed *= 2;
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

                inUse = true;

                attackZone.enabled = true;

                lastStart = Time.realtimeSinceStartup;

                controller.speed /= 2;

                particles.Play();

                camShaker.shakeCamera(shakeDuration, shakeMagnitude);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        LivingThing l = other.gameObject.GetComponentInParent<LivingThing>();
        if (l != null)
        {
            l.takeDamage(damage);
        }
    }
}
