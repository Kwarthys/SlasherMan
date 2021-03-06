﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionAnimationController : MonoBehaviour
{
    private Light attachedLight;
    private Renderer theRenderer;
    public float expansionTime = .1f;
    public float explosionDuration = .3f;
    public float destructionTimer = 1;
    public float lightStartingIntensity = 3;
    private float spawnTime;

    private Vector3 originalScale;

    private bool terminated = false;

    // Start is called before the first frame update
    void Start()
    {
        theRenderer = GetComponent<Renderer>();
        attachedLight = GetComponentInChildren<Light>();
        spawnTime = Time.realtimeSinceStartup;

        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (terminated) return;

        float expT = (Time.realtimeSinceStartup - spawnTime) / expansionTime;

        if(expT < 1)
        {
            transform.localScale = originalScale * expT;
        }


        float t = (Time.realtimeSinceStartup - spawnTime) / explosionDuration;

        if(t > 1)
        {
            terminated = true;
            Destroy(gameObject, destructionTimer - explosionDuration);
            return;
        }

        attachedLight.intensity = Mathf.Lerp(lightStartingIntensity, 0, t);
        theRenderer.material.SetFloat("_FadeAmount", t);

    }
}
