﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource center = null;
    [SerializeField]
    private AudioSource left = null;
    [SerializeField]
    private AudioSource right = null;

    public Transform player;

    public Slider mainVolume;

    public float ambianceVolumeCoef = 1;

    private float centerZoneSize = 20;

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void FixedUpdate()
    {
        transform.position = player.position;
    }

    public void playClip(AudioClip clip, float volume, Vector3 sourcePos)
    {
        AudioSource source;

        sourcePos.z = 0;
        Vector3 pos = transform.position;
        pos.z = 0;
        
        if((sourcePos - pos).sqrMagnitude < centerZoneSize)
        {
            source = center;
        }
        else if(sourcePos.x > pos.x)
        {
            source = right;
        }
        else
        {
            source = left;
        }

        if(source!=null)
        {
            //if(!source.isPlaying) //For now i'll check how stacking everything will work
            //{
                source.PlayOneShot(clip, volume * mainVolume.value * ambianceVolumeCoef);
            //}
        }
    }
}
