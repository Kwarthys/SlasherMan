using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public Vector3 axis = Vector3.up;

    public float speed = 50;


    void Update()
    {
        transform.Rotate(axis, speed * Time.deltaTime);
    }
}
