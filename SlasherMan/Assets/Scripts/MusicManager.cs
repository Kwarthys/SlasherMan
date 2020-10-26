using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource musicSource;

    public SpawnerManager monsterHolder;

    public float volumeMin = .5f;
    public float volumeMax = .9f;

    void FixedUpdate()
    {
        float t = monsterHolder.transform.childCount * 1.0f / monsterHolder.maxMonsterCount;

        musicSource.volume = Mathf.Lerp(volumeMin, volumeMax, t);
    }
}
