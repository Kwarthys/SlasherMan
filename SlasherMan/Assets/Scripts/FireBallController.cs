using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallController : MonoBehaviour
{
    public FireBallAttack initiator;
    public GameObject fireBallExplosion;
    public AnimationCurve damageOverDistance;
    public float maxDistance = 1.5f;
    public float speed = 150;
    public float maxLifeTime = 2;
    private float spawnTime = -1;

    private void Start()
    {
        spawnTime = Time.realtimeSinceStartup;
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        if(Time.realtimeSinceStartup - spawnTime > maxLifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;
        if (other.tag == "Player") return;

        //dodamage
        explode();
        initiator.dealDamage(other.GetComponentInParent<LivingThing>());

        //spawnAnimation
        //BOOM
        Instantiate(fireBallExplosion, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    private void explode()
    {
        initiator.shakeCamera();

        bool hit = false;

        foreach(Collider other in Physics.OverlapSphere(transform.position, maxDistance))
        {
            Debug.DrawRay(other.transform.position, Vector3.up * 5, Color.red, 50);
            if(!other.isTrigger && other.transform.tag != "Player")
            {
                //Debug.Log(other.tag);
                float realDistance = Vector3.Distance(transform.position, other.transform.position);
                float distanceCoef = damageOverDistance.Evaluate(realDistance / maxDistance / 2);
                int damageDone = (int)(initiator.damage * distanceCoef);
                //Debug.Log("Did " + damageDone + "(" + distanceCoef + ") damage to " + other.transform.name + " at " + realDistance);
                if(initiator.dealDamage(other.GetComponentInParent<LivingThing>(), damageDone))
                {
                    hit = true;
                }

            }
        }

        if(hit)
        {
            initiator.registerAddCharge();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }
}
