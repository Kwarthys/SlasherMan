using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashAttack : Ability
{
    public Camera cam;

    private BoxCollider attackZone;

    private int counter = 0;

    public GameObject anim;

    private Transform animSpawn;

    public LayerMask floorLayer;

    private void Start()
    {
        animSpawn = transform.Find("SlashAnimSpawn");
        attackZone = GetComponent<BoxCollider>();
        attackZone.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit " + other.gameObject.name);

        LivingThing l = other.gameObject.GetComponentInParent<LivingThing>();
        if (l != null)
        {
            l.takeDamage(damage);
        }
    }

    private void FixedUpdate()
    {
        if(inUse)
        {
            if(counter++ > 3)
            {
                counter = 0;
                inUse = false;
                attackZone.enabled = false;
            }
        }
    }

    private void Update()
    {
        if(canBeUsed())
        {
            if(Input.GetMouseButtonDown(0))
            {
                registerUse();
                inUse = true;
                attackZone.enabled = true;

                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                Debug.DrawRay(ray.origin, ray.direction);

                if (Physics.Raycast(ray, out hit, 60, floorLayer))
                {
                    Vector3 target = hit.point;
                    target.y = transform.parent.position.y;
                    transform.parent.rotation = Quaternion.LookRotation(target - transform.parent.position);
                }
                else
                {
                    Debug.Log("Could not find floor");
                }

                //play anim
                Instantiate(anim, animSpawn.position, animSpawn.rotation);
            }
        }
    }
}
