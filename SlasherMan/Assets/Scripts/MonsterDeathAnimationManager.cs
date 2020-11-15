using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDeathAnimationManager : MonoBehaviour
{
    public Renderer monsterRenderer;

    public Material deathMaterial;

    public float animDuration = .5f;
    private float animStart = -1;

    // Update is called once per frame
    void Update()
    {
        if (animStart == -1) return;

        float t = (Time.realtimeSinceStartup - animStart) / animDuration;

        if(t>1)
        {
            Destroy(gameObject);
            return;
        }

        for (int i = 0; i < monsterRenderer.materials.Length; ++i)
        {
            monsterRenderer.materials[i].SetFloat("_FadeAmount", t);
        }
    }

    public void startAnimation()
    {
        if (animStart != -1) return;

        Material[] newMats = new Material[monsterRenderer.materials.Length];

        for(int i = 0; i < monsterRenderer.materials.Length; ++i)
        {
            newMats[i] = deathMaterial;
            monsterRenderer.materials[i].SetFloat("_FadeAmount", 0);
        }

        monsterRenderer.materials = newMats;

        animStart = Time.realtimeSinceStartup;
    }
}
