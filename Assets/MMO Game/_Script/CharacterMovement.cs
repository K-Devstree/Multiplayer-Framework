using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    CharacterController characterController;

    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Animator CharacterAnimator;
    private Vector3 moveDirection = Vector3.zero;

    void Start()
    {
        //characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        //if (characterController.isGrounded)
        //{
        //    moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        //    moveDirection *= speed;

        //    if (Input.GetButton("Jump"))
        //    {
        //        moveDirection.y = jumpSpeed;
        //    }
        //}
        //moveDirection.y -= gravity * Time.deltaTime;
        //characterController.Move(moveDirection * Time.deltaTime);

        if (Input.GetKeyUp(KeyCode.Z))
        {
            CharacterAnimator.SetTrigger("idle");
        }
        else if (Input.GetKeyUp(KeyCode.X))
        {
            CharacterAnimator.SetTrigger("walk");
        }
        else if (Input.GetKeyUp(KeyCode.C))
        {
            CharacterAnimator.SetTrigger("run");
        }
        else if (Input.GetKeyUp(KeyCode.B))
        {
            CharacterAnimator.SetTrigger("jump");
        }
        else if (Input.GetKeyUp(KeyCode.N))
        {
            CharacterAnimator.SetTrigger("injured");
        }
        else if (Input.GetKeyUp(KeyCode.M))
        {
            CharacterAnimator.SetTrigger("attack");
        }
        else if (Input.GetKeyUp(KeyCode.L))
        {
            CharacterAnimator.SetTrigger("death");
        }
        else if (Input.GetKeyUp(KeyCode.K))
        {
            CharacterAnimator.SetTrigger("revive");
        }
    }
}
