using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class MonsterAbility : MonoBehaviour
{
    public float monsterAttackCD = 1;
    public float lastCast = -1;

    public float timeBeforeAttack = 1;
    private float timeOfAttackStart = -1;
    private bool attacking = false;

    public GameObject preAttackAnim;
    private HighLightResizer preAttackAnimInstanciated;

    public GameObject attackAnim;

    public Animator monsterAnimator;

    public int damage = 20;

    public bool friendlyFire = false;

    [Header("Animation")]
    public bool rotateAnimation = false;
    public bool forwardAnimation = true;

    [Header("Audio")]
    public float clipVolume = 1f;
    private static AudioManager audioManager;
    public List<AudioClip> attacksAudio = new List<AudioClip>();
    
    private BoxCollider attackZone;

    private Monster controller;

    private int counter = 0;

    private bool freezeDamage = false;

    // Start is called before the first frame update
    void Start()
    {
        attackZone = GetComponent<BoxCollider>();
        attackZone.enabled = false;

        controller = transform.parent.GetComponent<Monster>();

        audioManager = AudioManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Trigger the attack
        //Debug.Log("detected " + other.transform.root.name);

        if (freezeDamage) return;

        if (other.isTrigger) return;

        if(other.transform.root.tag == "Player" || friendlyFire)
        {
            LivingThing l = other.GetComponentInParent<LivingThing>();
            if(l!=null)
            {
                l.takeDamage(damage);
                //Debug.Log("hit " + other.transform.root.name);
            }
        }
    }

    private void Update()
    { 
        if(attacking)
        {
            if (Time.realtimeSinceStartup - timeOfAttackStart > timeBeforeAttack)
            {
                attacking = false;
                attackZone.enabled = true;

                //AttackAnim
                //Debug.Log("Attacking");
                if(attackAnim!=null)
                {
                    Vector3 pos = transform.position;
                    if(forwardAnimation)
                    {
                        pos += transform.forward * 1.5f;
                    }

                    Quaternion rot = rotateAnimation ? transform.rotation : Quaternion.identity;

                    Instantiate(attackAnim, pos, rot);
                }

                Destroy(preAttackAnimInstanciated.gameObject);

                if(attacksAudio.Count > 0 && audioManager!=null)
                {
                    AudioClip clip = attacksAudio[Random.Range(0, attacksAudio.Count)];
                    audioManager.playClip(clip, clipVolume, transform.position);
                }
            }
        }
    }

    public void startAttack()
    {
        attacking = true;
        timeOfAttackStart = Time.realtimeSinceStartup;

        controller.setAllowMovement(false);

        //StartAttackAnim
        //Debug.Log("StartAttack");
        freezeDamage = true;
        attackZone.enabled = true;
        preAttackAnimInstanciated = Instantiate(preAttackAnim, attackZone.bounds.center, transform.rotation, transform).GetComponent<HighLightResizer>();
        preAttackAnimInstanciated.resize(attackZone.size);
        freezeDamage = false;
        attackZone.enabled = false;

        if(monsterAnimator != null)
        {
            monsterAnimator.SetTrigger("Attack");
        }
    }

    private void FixedUpdate()
    {
        if(attackZone.enabled == true)
        {
            if(counter++ > 3)
            {
                attackZone.enabled = false;
                counter = 0;
                controller.setAllowMovement(true);
            }
        }
    }
}
