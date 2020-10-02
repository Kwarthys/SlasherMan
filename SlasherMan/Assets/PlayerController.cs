using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 1;

    private float threshold = 0.15f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = remap8(Input.GetAxisRaw("Horizontal"));
        float vertical = remap8(Input.GetAxisRaw("Vertical"));

        if(Mathf.Abs(horizontal) > threshold || Mathf.Abs(vertical) > threshold)
        {
            Vector3 movement = new Vector3(horizontal, 0, vertical);

            if(Mathf.Abs(horizontal) == 1 && Mathf.Abs(vertical) == 1)
            {
                movement /= Mathf.Sqrt(2);
            }

            transform.position += movement * speed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(movement);
        }

    }


    private float remap8(float f)
    {
        if(f > threshold)
        {
            return 1;
        }
        else if(f < -threshold)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }
}
