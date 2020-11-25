using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingThing : MonoBehaviour
{
    public int maxLife;

    public GameObject deathAnimation;

    [Header("Audio")]
    public List<AudioClip> onDamageClips = new List<AudioClip>();
    public float clipsVolume = 1;
    public float cooldown = 1.5f;
    private float lastOnDamage = -1;
    private static AudioManager audioManager;
    [Space]
    [SerializeField]
    protected int life;

    [Header("Effects")]
    public int bonusArmor = 0;

    private bool dead = false;

    public void changeBonusLife(int amount)
    {
        maxLife += amount;

        if(amount > 0)
        {
            life += amount;
        }

        life = Mathf.Min(maxLife, life);
    }

    public int getCurrentLife() { return life; }

    private void Start()
    {
        life = maxLife;

        if (audioManager == null)
        {
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        }

        init();
    }

    public void takeDamage(int amount)
    {
        life -= Mathf.Max(1,amount + bonusArmor); //can't take less than 1 damage

        if (onDamageClips.Count > 0 && audioManager != null && Time.realtimeSinceStartup - lastOnDamage > cooldown)
        {
            lastOnDamage = Time.realtimeSinceStartup;
            AudioClip clip = onDamageClips[Random.Range(0, onDamageClips.Count)];
            audioManager.playClip(clip, clipsVolume, transform.position);
        }

        onTakeDamage();
    }

    protected virtual void onHeal() { }

    public void heal(int amount)
    {
        life = Mathf.Min(maxLife, life + amount);
        onHeal();
    }

    public void reinit()
    {
        dead = false;
        life = maxLife;
    }

    private void Update()
    {
        if (life <= 0 && !dead)
        {
            dead = true;
            onDeath();
        }

        onUpdate();
    }

    public virtual void init() {}

    protected virtual void onDeath() { }
    protected virtual void onTakeDamage() { }
    protected virtual void onUpdate() { }
}
