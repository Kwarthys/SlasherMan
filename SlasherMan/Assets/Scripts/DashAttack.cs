using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : Ability
{
    public float speed = 20;
    public float dashDistance = 7;

    public LayerMask enemiesLayer;

    private Vector3 dashTarget;

    private Rigidbody rbody;

    private Transform parent;

    public LayerMask walls;

    protected override void onStart()
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

    protected override bool inputPressed()
    {
        return MyInputManager.Instance.dashKeyPressed();
    }

    protected override void cast()
    {
        inUse = true;
        controller.canMove = false;
        attackZone.enabled = true;
        rbody.isKinematic = true;

        camShaker.shakeCamera(shakeDuration, shakeMagnitude);

        dashTarget = MyInputManager.Instance.getMoveDirection();
        if (dashTarget.magnitude < 0.1f)
        {
            dashTarget = transform.position + transform.forward * dashDistance;
        }
        else
        {
            dashTarget = transform.position + dashTarget * dashDistance;
        }

        Instantiate(anim, transform.position, transform.rotation);

        playerAnimator.SetTrigger("Dash");
    }

    protected override void registerToManager()
    {
        manager.registerDash();
    }

    private void stopDash()
    {
        inUse = false;
        controller.canMove = true;
        rbody.isKinematic = false;
        attackZone.enabled = false;
        manager.releaseAttackBlock();
    }

    protected override void onUpdate()
    {
        if(inUse)
        {
            if(!Physics.Raycast(transform.position, transform.forward, 3.0f, walls))
            {
                //Debug.DrawRay(transform.position, transform.forward * 3, Color.green, 1);
                parent.position += (dashTarget - parent.position) * speed * Time.deltaTime;
            }
            else
            {
                stopDash();
            }

            if(Vector3.Distance(parent.position, dashTarget) < 0.1f)
            {
                stopDash();
            }
            return;
        }
        
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, dashDistance);
    }
}
