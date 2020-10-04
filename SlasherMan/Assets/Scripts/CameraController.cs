using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, 15, -30);

    public Transform target;

    private Vector3 shakeTarget;

    private void Start()
    {
        transform.rotation = Quaternion.LookRotation(target.position - transform.position);
    }

    void Update()
    {
        transform.position = target.position + offset;
    }

    private void OnValidate()
    {
        Start();
        Update();
    }
}
