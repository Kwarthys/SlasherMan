using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadAimAnimator : MonoBehaviour
{
    public Transform headTarget;

    public float headTargetHeight = 1.5f;

    public float headSpeed = 30;

    private Vector3 headCurrentTarget = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        float step = headSpeed * Time.deltaTime;

        if((headTarget.position - headCurrentTarget).magnitude < step)
        {
            headTarget.position = headCurrentTarget;
        }
        else
        {
            headTarget.position += (headCurrentTarget - headTarget.position) * step;
        }
    }

    private void FixedUpdate()
    {
        registerHeadAim();
    }

    private void registerHeadAim()
    {
        Vector3 pos = Vector3.zero;

        if (MyInputManager.Instance.tryGetAimDirection(out Vector3 aim))
        {
            if (Vector3.Angle(aim, transform.forward) > 100)
            {
                aim = transform.forward;
            }

            pos = transform.position + aim;

        }
        else
        {
            pos = transform.position + transform.forward;
        }

        pos.y += headTargetHeight;

        headCurrentTarget = pos;
    }
}
