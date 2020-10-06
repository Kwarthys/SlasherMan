using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    [Header("Camera Shake")]
    public float shakeDuration;
    public float shakeMagnitude;
    public CameraShake camShaker;
    [Space]
    public Camera cam;
    public LayerMask floorLayer;
    [Space]
    public bool allowed = true;

    public string abilityName;

    public AttackManager manager;

    public GameObject anim;

    public float internalCD;
    private float lastCast = 0;

    protected Collider attackZone;

    public int damage;

    protected bool inUse = false;

    public bool canBeUsed()
    {
        return allowed && (Time.realtimeSinceStartup - lastCast > internalCD);
    }

    protected void registerUse()
    {
        lastCast = Time.realtimeSinceStartup;

        if(manager == null)
        {
            manager = GetComponentInParent<AttackManager>();
        }
    }

    protected bool tryFindTarget(out Vector3 target)
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 60, floorLayer))
        {
            target = hit.point;
            target.y = transform.parent.position.y;
            return true;
        }

        target = Vector3.zero;
        return false;
    }
}
