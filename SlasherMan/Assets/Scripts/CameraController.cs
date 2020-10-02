using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, 15, -30);

    public Transform target;

    void Start()
    {
        
    }

    void Update()
    {
        transform.position = target.position + offset;
        transform.rotation = Quaternion.LookRotation(target.position - transform.position);
    }
}
