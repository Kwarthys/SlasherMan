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
    public bool allowed = true;

    public GameObject anim;

    public float internalCD;
    private float lastCast = 0;

    public int damage;

    protected bool inUse = false;

    protected bool canBeUsed()
    {
        return allowed && (Time.realtimeSinceStartup - lastCast > internalCD);
    }

    protected void registerUse()
    {
        lastCast = Time.realtimeSinceStartup;
    }
}
