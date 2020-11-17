using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmbianceSoundManager : MonoBehaviour
{
    public Slider masterSound;

    public List<AudioClip> clips = new List<AudioClip>();
    public float clipVolume;

    public AudioSource source;

    public float timeBetween = 30;
    public float randomTime = 10;
    private float nextSound = 0;
    private float lastSound = 10;

    void Update()
    {
        if (Time.realtimeSinceStartup - lastSound > nextSound)
        {
            float rand = Random.value;
            if(rand < 0.4f)
            {
                source.panStereo = -1;
            }
            else if(rand < 0.6)
            {
                source.panStereo = 0;
            }
            else
            {
                source.panStereo = 1;
            }

            AudioClip clip = clips[Random.Range(0, clips.Count)];
            source.PlayOneShot(clip, clipVolume * masterSound.value);

            lastSound = Time.realtimeSinceStartup;
            nextSound = timeBetween + Random.Range(0, randomTime);
        }
    }
}
