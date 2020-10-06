using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, 15, -30);

    public Transform target;

    public float cameraSpeed = 20;

    private Vector3 shakeTarget;

    private void Start()
    {
        transform.rotation = Quaternion.LookRotation(target.position - transform.position);
    }

    void Update()
    {
        transform.position += (target.position + offset - transform.position) * cameraSpeed * Time.deltaTime;
    }

    private void OnValidate()
    {
        Start();
        Update();
    }
}
