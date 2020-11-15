using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    [Header("Sources")]
    public AudioSource musicSource;
    public AudioSource bassSource;

    [Header("Animation")]
    public float transiTime = 1;
    private float transiStart = -10;
    private bool music = true;

    [Space]

    public SpawnerManager monsterHolder;

    public Slider mainVolume;

    public float volumeMin = .5f;
    public float volumeMax = .9f;

    void FixedUpdate()
    {
        float transiCoef = (Time.realtimeSinceStartup - transiStart) / transiTime;
        transiCoef = Mathf.Min(1, transiCoef);

        float bassCoef;
        float musicCoef;

        if(music)
        {
            musicCoef = transiCoef;
            bassCoef = 1 - transiCoef;
        }
        else
        {
            musicCoef = 1 - transiCoef;
            bassCoef = transiCoef;
        }

        float t = monsterHolder.transform.childCount * 1.0f / monsterHolder.maxMonsterCount;
        musicSource.volume = Mathf.Lerp(volumeMin, volumeMax, t) * mainVolume.value * musicCoef;

        bassSource.volume = bassCoef;
    }

    public void transiToBass()
    {
        music = false;
        transiStart = Time.realtimeSinceStartup;
    }

    public void transiToMusic()
    {
        music = true;
        transiStart = Time.realtimeSinceStartup;
    }
}
