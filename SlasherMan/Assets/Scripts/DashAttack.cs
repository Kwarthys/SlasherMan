using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : Ability
{

    public PlayerController controller;

    public float speed = 20;
    public float dashDistance = 7;

    public LayerMask enemiesLayer;

    private Vector3 dashTarget;

    private Rigidbody rbody;

    private Transform parent;

    private void Start()
    {
        rbody = GetComponentInParent<Rigidbody>();
        parent = transform.parent;

        attackZone = GetComponent<SphereCollider>();
        attackZone.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;
        dealDamage(other.gameObject.GetComponentInParent<LivingThing>());       
    }


    void Update()
    {
        if(inUse)
        {
            parent.position += (dashTarget - parent.position) * speed * Time.deltaTime;

            if(Vector3.Distance(parent.position, dashTarget) < 0.1f)
            {
                inUse = false;
                controller.canMove = true;
                rbody.isKinematic = false;
                attackZone.enabled = false;
                manager.releaseAttackBlock();
            }
            return;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(canBeUsed())
            {
                //cast
                registerUse();
                manager.registerDash();
                startSoundEffect();

                inUse = true;
                controller.canMove = false;
                attackZone.enabled = true;
                rbody.isKinematic = true;

                Vector3 hit;

                if (tryFindTarget(out hit))
                {
                    dashTarget = hit;

                    //if(Vector3.Distance(dashTarget, parent.position) > dashDistance)
                    //{
                    Vector3 dashDirection = (dashTarget - parent.position).normalized;
                    dashTarget = parent.position + dashDirection * dashDistance;
                    //}
                }
                else
                {
                    Debug.LogError("Couldn't find floor to dash");
                    inUse = false;
                }

                Debug.DrawRay(parent.position, dashTarget - parent.position, Color.green, 1);

                parent.rotation = Quaternion.LookRotation(dashTarget - parent.position);

                Instantiate(anim, transform.position, transform.rotation);

                //playerAnimator.SetTrigger("Dash");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, dashDistance);
    }
}
