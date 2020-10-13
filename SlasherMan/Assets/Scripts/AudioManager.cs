using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource center;
    [SerializeField]
    private AudioSource left;
    [SerializeField]
    private AudioSource right;

    public Transform player;

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
                source.PlayOneShot(clip, volume);
            //}
        }
    }
}
