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

    [Header("OnHitScreenAnimation")]
    public Image onHitImage;
    public float maxOpacity = .7f;
    public float decreaseOpacity = 0.15f;
    private float opacity = 0;
    private Color color = Color.white;

    public bool hasTie = false;


    protected override void onUpdate()
    {
        if(opacity > 0)
        {
            opacity -= decreaseOpacity;

            if (opacity <= 0)
            {
                opacity = 0;
            }

            color.a = opacity;
            onHitImage.color = color;
        }
    }

    protected override void onTakeDamage()
    {
        //Take damage animations (sound / visual / camShake)
        slider.value = life * 1.0f / maxLife;

        camShake.shakeCamera(duration, magnitude);

        opacity = maxOpacity;
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
        reinit();
        slider.value = life * 1.0f / maxLife;

    }

    public void replace()
    {
        transform.position = Vector3.zero;
    }

    protected override void onHeal()
    {
        slider.value = life * 1.0f / maxLife;
    }
}
