using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public PlayerController controller;
    [Header("Camera Shake")]
    public float shakeDuration;
    public float shakeMagnitude;
    public CameraShake camShaker;
    [Space]
    public Camera cam;
    public LayerMask floorLayer;
    [Header("Audio")]
    public List<AudioClip> clips = new List<AudioClip>();
    public List<AudioClip> clipsNoHit = new List<AudioClip>();
    public float volume = 1;
    public float volumeNoHit = 1;
    public AudioManager audioManager;
    [Header("Stats")]
    public string abilityName;
    public int totalDamage = 0;
    public int totalKills = 0;
    [Space]
    public bool allowed = true;
    public float attackDelay = 0;
    private bool waitingDelay = false;

    protected Vector3 aimDir;

    public AttackManager manager;

    public GameObject anim;

    public float internalCD;
    protected float lastCast = 0;

    protected Collider attackZone;

    public int damage;

    public Animator playerAnimator;

    protected bool inUse = false;

    private void Update()
    {
        if(canBeCasted() && inputPressed())
        {
            registerCast();
            registerToManager();
            startSoundEffect();
            if(attackDelay == 0)
            {
                cast();
            }
            else
            {
                prepareCast();
                waitingDelay = true;
            }
        }

        if(waitingDelay)
        {
            if(Time.realtimeSinceStartup - lastCast > attackDelay)
            {
                waitingDelay = false;
                cast();
            }
        }

        onUpdate();
    }

    protected abstract void registerToManager();

    protected abstract void cast();

    protected virtual void prepareCast() { }

    protected virtual void onUpdate() { }

    public bool canBeCasted()
    {
        return allowed && (Time.realtimeSinceStartup - lastCast > internalCD) && !manager.isAttackBlocked();
    }

    protected void registerCast()
    {
        lastCast = Time.realtimeSinceStartup;

        if(manager == null)
        {
            manager = GetComponentInParent<AttackManager>();
        }
    }

    protected abstract bool inputPressed();

    protected void startSoundEffect()
    {
        if (clips.Count > 0 && audioManager != null)
        {
            AudioClip clip = clips[Random.Range(0, clips.Count)];
            audioManager.playClip(clip, volume, transform.position);
        }
    }

    protected void startSoundEffectNoHit()
    {
        if (clipsNoHit.Count > 0 && audioManager != null)
        {
            AudioClip clip = clipsNoHit[Random.Range(0, clipsNoHit.Count)];
            audioManager.playClip(clip, volumeNoHit, transform.position);
        }
    }

    protected bool tryFindAimDirection(out Vector3 target)
    {
        return MyInputManager.Instance.tryGetAimDirection(out target);
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

    protected void newSteerToAim()
    {
        if (MyInputManager.Instance.tryGetAimDirection(out aimDir))
        {
            //yee we have an aim
        }
        else
        {
            aimDir = transform.forward;
        }
    }

    protected void steerToAim()
    {
        if (tryFindAimDirection(out Vector3 dir))
        {
            aimDir = dir;
        }

        transform.parent.rotation = Quaternion.LookRotation(aimDir);
    }
}
