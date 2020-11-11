using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MyInputManager : MonoBehaviour
{
    public float threshold = 0.15f;

    public Camera cam;

    public LayerMask floor;

    public static MyInputManager Instance { get; private set; }

    private float mouseTimeOut = .5f;
    private float lastMouseMove = -1;

    public TextMeshProUGUI blazeKeyHint;
    public TextMeshProUGUI slashKeyHint;
    public TextMeshProUGUI dashKeyHint;

    private Vector3 lastMousePos = Vector3.zero;

    private void Awake()
    {
        Instance = this;        
    }

    public bool attackKeyPressed()
    {
        return Input.GetMouseButtonDown(0) || Mathf.Abs(Input.GetAxisRaw("RightTrigger")) > threshold;
    }

    public bool supportKeyPressed()
    {
        return Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0);
    }

    public bool specialKeyPressed()
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

        bool mouseUsed = !isMouseUseless();

        if(!mouseUsed && blazeKeyHint.text == "R")
        {
            blazeKeyHint.text = "Left Trigger";
            slashKeyHint.text = "Right Trigger";
            dashKeyHint.text = "A";

            Cursor.visible = false;
        }
        else if(mouseUsed && blazeKeyHint.text == "Left Trigger")
        {
            blazeKeyHint.text = "R";
            slashKeyHint.text = "Left Clic";
            dashKeyHint.text = "Space";

            Cursor.visible = true;
        }

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
        bool keyBoardUsed = Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        return Time.realtimeSinceStartup - lastMouseMove > mouseTimeOut && !keyBoardUsed;
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
