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
    [Header("Audio")]
    public List<AudioClip> clips = new List<AudioClip>();
    public float volume = 1;
    public AudioManager audioManager;
    [Header("Stats")]
    public string abilityName;
    public int totalDamage = 0;
    public int totalKills = 0;
    [Space]
    public bool allowed = true;

    public AttackManager manager;

    public GameObject anim;

    public float internalCD;
    private float lastCast = 0;

    protected Collider attackZone;

    public int damage;

    public Animator playerAnimator;

    protected bool inUse = false;

    public bool canBeUsed()
    {
        return allowed && (Time.realtimeSinceStartup - lastCast > internalCD) && !manager.isAttackBlocked();
    }

    protected void registerUse()
    {
        lastCast = Time.realtimeSinceStartup;

        if(manager == null)
        {
            manager = GetComponentInParent<AttackManager>();
        }
    }

    protected void startSoundEffect()
    {
        if(clips.Count>0 && audioManager != null)
        {
            AudioClip clip = clips[Random.Range(0, clips.Count)];
            audioManager.playClip(clip, volume, transform.position);
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

    protected void dealDamage(LivingThing target)
    {
        if (target != null)
        {
            totalDamage += Mathf.Min(damage, target.getCurrentLife());
            target.takeDamage(damage);
            if (target.getCurrentLife() <= 0)
            {
                totalKills++;
            }
        }
    }
}
