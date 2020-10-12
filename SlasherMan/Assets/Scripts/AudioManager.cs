using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource center;
    private AudioSource left;
    private AudioSource right;

    public Transform player;

    private float centerZoneSize = 20;

    void Start()
    {
        center = transform.Find("Center").GetComponent<AudioSource>();
        left = transform.Find("Left").GetComponent<AudioSource>();
        right = transform.Find("Right").GetComponent<AudioSource>();
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
            Debug.Log("center d:" + (sourcePos - pos).sqrMagnitude + " < " + centerZoneSize);
        }
        else if(sourcePos.x > pos.x)
        {
            source = right;
            Debug.Log("right d:" + (sourcePos - pos).sqrMagnitude);
        }
        else
        {
            source = left;
            Debug.Log("left d:" + (sourcePos - pos).sqrMagnitude);
        }

        if(source!=null)
        {
            //if(!source.isPlaying) //For now i'll check how stacking everything will work
            //{
                source.PlayOneShot(clip, volume);
            //}
        }
    }
}
