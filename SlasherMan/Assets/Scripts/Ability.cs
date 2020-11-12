using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    protected PlayerController controller;
    [Header("Camera Shake")]
    public float shakeDuration;
    public float shakeMagnitude;
    protected CameraShake camShaker;
    [Space]
    protected Camera cam;
    public LayerMask floorLayer;
    [Header("Audio")]
    public List<AudioClip> clips = new List<AudioClip>();
    public List<AudioClip> clipsNoHit = new List<AudioClip>();
    public float volume = 1;
    public float volumeNoHit = 1;
    protected AudioManager audioManager;
    [Header("Stats")]
    public string abilityName;
    public int totalDamage = 0;
    public int totalKills = 0;
    [Space]
    public float attackDelay = 0;
    private bool waitingDelay = false;

    protected Vector3 aimDir;

    protected AttackManager manager;

    [Header("AttackParameters")]
    public GameObject anim;

    public float internalCD;
    protected float lastCast = 0;

    protected Collider attackZone;

    public int damage;

    public int chargeCost = 0;
    public int chargeCapacity = 0;
    public int chargeAmount = 0;

    protected Animator playerAnimator;

    protected bool inUse = false;

    private void Start()
    {
        controller = GetComponentInParent<PlayerController>();
        manager = GetComponentInParent<AttackManager>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        camShaker = cam.gameObject.GetComponent<CameraShake>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        playerAnimator = GameObject.Find("Hero").GetComponent<Animator>();

        onStart();
    }

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

    protected virtual void onStart() { }

    protected abstract void registerToManager();

    public void registerAddCharge() { manager.registerAddCharge(); }

    protected abstract void cast();

    protected virtual void prepareCast() { }

    protected virtual void onUpdate() { }

    public bool canBeCasted()
    {
        if (manager == null) return false;//happens before the "start" of the new abilities
        return (Time.realtimeSinceStartup - lastCast > internalCD) && !manager.isAttackBlocked() && chargeAmount >= chargeCost;
    }

    protected void registerCast()
    {
        lastCast = Time.realtimeSinceStartup;
    }

    public void addCharge()
    {
        chargeAmount = Mathf.Min(chargeAmount + 1, chargeCapacity);
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

    public bool dealDamage(LivingThing target, int damageAmount)
    {
        if (target != null)
        {
            totalDamage += Mathf.Min(damageAmount, target.getCurrentLife());
            target.takeDamage(damageAmount);
            if (target.getCurrentLife() <= 0)
            {
                totalKills++;
            }
        }

        return target != null;
    }

    public void dealDamage(LivingThing target)
    {
        dealDamage(target, damage);
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
