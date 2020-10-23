using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LivingThing : MonoBehaviour
{
    public int maxLife;

    public GameObject deathAnimation;

    [Header("Audio")]
    public List<AudioClip> onDamageClips = new List<AudioClip>();
    public float clipsVolume = 1;
    private static AudioManager audioManager;
    [Space]
    [SerializeField]
    protected int life;

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
        life -= amount;

        if (onDamageClips.Count > 0 && audioManager != null)
        {
            AudioClip clip = onDamageClips[Random.Range(0, onDamageClips.Count)];
            audioManager.playClip(clip, clipsVolume, transform.position);
        }

        onTakeDamage();
    }

    private void Update()
    {
        if (life <= 0)
        {
            onDeath();
        }

        onUpdate();
    }

    public abstract void init();

    protected virtual void onDeath() { }
    protected virtual void onTakeDamage() { }
    protected virtual void onUpdate() { }
}
