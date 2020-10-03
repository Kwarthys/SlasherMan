using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : MonoBehaviour
{
    public Camera cam;

    public bool isOkay = true;

    public PlayerController controller;

    public float internalCD = 0.3f;
    private float lastCast = -1;

    public float speed = 20;
    public float dashDistance = 7;

    public LayerMask floorLayer;
    public LayerMask enemiesLayer;

    public int damage = 25;

    private bool dashing = false;
    private Vector3 dashTarget;

    private Rigidbody rbody;

    private Transform parent;

    private void Start()
    {
        rbody = GetComponentInParent<Rigidbody>();
        parent = transform.parent;
    }

    private void OnTriggerEnter(Collider col)
    {
        if(dashing)
        {
            LivingThing l = col.gameObject.GetComponentInParent<LivingThing>();
            if(l!=null)
            {
                l.takeDamage(damage);
            }
        }
    }


    void Update()
    {
        if(dashing)
        {
            parent.position += (dashTarget - parent.position) * speed * Time.deltaTime;

            if(Vector3.Distance(parent.position, dashTarget) < 0.1f)
            {
                dashing = false;
                controller.canMove = true;
                rbody.isKinematic = false;
            }
            return;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(Time.realtimeSinceStartup - lastCast > internalCD)
            {
                //cast
                lastCast = Time.realtimeSinceStartup;
                dashing = true;
                controller.canMove = false;

                rbody.isKinematic = true;

                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                Debug.DrawRay(ray.origin, ray.direction);

                if (Physics.Raycast(ray, out hit, 60, floorLayer))
                {
                    dashTarget = hit.point;
                    dashTarget.y += 1;

                    if(Vector3.Distance(dashTarget, parent.position) > dashDistance)
                    {
                        Vector3 dashDirection = (dashTarget - parent.position).normalized;
                        dashTarget = parent.position + dashDirection * dashDistance;
                    }
                }
                else
                {
                    Debug.LogError("Couldn't find floor to dash");
                    dashing = false;
                }

                Debug.DrawRay(parent.position, dashTarget - parent.position, Color.green, 1);

                parent.rotation = Quaternion.LookRotation(dashTarget - parent.position);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, dashDistance);
    }
}
