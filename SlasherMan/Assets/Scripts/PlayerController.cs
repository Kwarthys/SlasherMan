using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 1;

    private float threshold = 0.15f;

    private Rigidbody rbody;

    public bool canMove = true;

    public float rotSpeed = 10;

    public Animator playerAnimator;

    public LayerMask everything;

    void Start()
    {
        rbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        bool isRunning = true;

        if(!canMove)
        {
            return;
        }

        Vector3 movement = MyInputManager.Instance.getMoveDirection();

        if (movement.magnitude > threshold)
        {
            movement.Normalize();

            Quaternion targetRot = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotSpeed);

            /**Check Obstacles**/
            bool centerObstacle = Physics.Raycast(transform.position, transform.forward, 1, everything, QueryTriggerInteraction.Ignore);

            if (!centerObstacle)
            {
                transform.position += movement * speed * Time.deltaTime;
            }

        }
        else// if (vertical == 0 && horizontal == 0) //trying this
        {
            isRunning = false;
        }

        rbody.velocity = Vector3.zero;
        playerAnimator.SetBool("isRunning", isRunning);
    }
}
