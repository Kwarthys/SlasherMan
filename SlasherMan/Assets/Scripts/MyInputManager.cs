using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyInputManager : MonoBehaviour
{
    public float threshold = 0.15f;

    public Camera cam;

    public LayerMask floor;

    public static MyInputManager Instance { get; private set; }

    private float mouseTimeOut = .5f;
    private float lastMouseMove = -1;

    private Vector3 lastMousePos = Vector3.zero;

    private void Awake()
    {
        Instance = this;        
    }

    public bool slashKeyPressed()
    {
        return Input.GetMouseButtonDown(0) || Mathf.Abs(Input.GetAxisRaw("RightTrigger")) > threshold;
    }

    public bool dashKeyPressed()
    {
        return Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0);
    }

    public bool blazeKeyPressed()
    {
        return Input.GetKeyDown(KeyCode.R) || Mathf.Abs(Input.GetAxisRaw("LeftTrigger")) > threshold;
    }

    public Vector3 getMoveDirection()
    {
        float horizontal = remap8(Input.GetAxisRaw("Horizontal"));
        float vertical = remap8(Input.GetAxisRaw("Vertical"));

        return new Vector3(horizontal, 0, vertical).normalized;

        //Debug.DrawRay(Vector3.zero, moveDirection, Color.green);
    }

    public bool tryGetAimDirection(out Vector3 aimDir)
    {
        if(!isMouseUseless())
        {
            return tryFindTargetMouse(out aimDir);
        }
        else
        {
            return tryGetAimDirectionStick(out aimDir);
        }
    }

    private bool tryGetAimDirectionStick(out Vector3 aimDir)
    {
        //RightStick
        float shorizontal = remap8(Input.GetAxisRaw("SecondHorizontal"));
        float svertical = remap8(Input.GetAxisRaw("SecondVertical"));

        aimDir = new Vector3(shorizontal, 0, svertical);

        return aimDir.magnitude > 0;

        //Debug.DrawRay(Vector3.zero, aimDirection, Color.blue);
    }

    private void Update()
    {
        //Debug.Log(Input.mousePosition + " " + isMouseUseless());
        if(Input.mousePosition != lastMousePos)
        {
            lastMouseMove = Time.realtimeSinceStartup;
        }
        lastMousePos = Input.mousePosition;

        /*
        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            Debug.Log("A");
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            Debug.Log("B");
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            Debug.Log("X");
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            Debug.Log("Y");
        }
        */
    }

    private bool isMouseUseless()
    {
        return Time.realtimeSinceStartup - lastMouseMove > mouseTimeOut;
    }

    protected bool tryFindTargetMouse(out Vector3 target)
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 60, floor))
        {
            target = hit.point;
            target.y = transform.position.y;

            target = target - transform.position;

            return true;
        }

        target = Vector3.zero;
        return false;
    }


    private float remap8(float f)
    {
        if (f > threshold)
        {
            return 1;
        }
        else if (f < -threshold)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }
}
