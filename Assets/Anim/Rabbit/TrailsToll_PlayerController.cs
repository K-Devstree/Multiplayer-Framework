using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailsToll_PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator PlayerAnimator;
    void Start()
    {
        //PlayerAnimator.SetTrigger("Idle");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Z))
        {
            PlayerAnimator.SetTrigger("Idle");
        }
        else if (Input.GetKeyUp(KeyCode.X))
        {
            PlayerAnimator.SetTrigger("Run");
        }
        else if (Input.GetKeyUp(KeyCode.C))
        {
            PlayerAnimator.SetTrigger("Up");
        }
        else if (Input.GetKeyUp(KeyCode.V))
        {
            PlayerAnimator.SetTrigger("Roll");
        }
        else if (Input.GetKeyUp(KeyCode.B))
        {
            PlayerAnimator.SetTrigger("Failure");
        }
    }
}
