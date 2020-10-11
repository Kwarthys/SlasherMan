using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingThing
{
    public Slider slider;

    [Header("OnDamageCameraShake")]
    public CameraShake camShake;
    public float duration = 0.2f;
    public float magnitude = 0.2f;

    [Header("GameManagement")]
    public GameManager manager;

    protected override void onTakeDamage()
    {
        //Take damage animations (sound / visual / camShake)
        slider.value = life * 1.0f / maxLife;

        camShake.shakeCamera(duration, magnitude);
    }

    protected override void onDeath()
    {
        if (deathAnimation != null)
        {
            Instantiate(deathAnimation, transform.position, Quaternion.identity);
        }      

        slider.value = life * 1.0f / maxLife;

        manager.notifyPlayerDead();
    }

    public override void init()
    {
        life = maxLife;
        slider.value = life * 1.0f / maxLife;

        transform.position = Vector3.zero;
    }
}
