using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    public MovementAdavanced movementScript;


    //to do: if npc is character,too: move to movement.cs
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        //input/movement part
        if (Input.GetKey("w"))
        {
            movementScript.direction = "forward";
            animator.SetBool("Run", true);
            animator.SetFloat("Walking", 1);
        }
        if (Input.GetKey("s"))
        {
            movementScript.direction = "back";
            animator.SetBool("Run", true);
            animator.SetFloat("Walking", 1);
        }
        if (Input.GetKey("a"))
        {
            movementScript.direction = "left";
            animator.SetBool("Run", true);
            animator.SetFloat("Walking", 1);
        }
        if (Input.GetKey("d"))
        {
            movementScript.direction = "right";
            animator.SetBool("Run", true);
            animator.SetFloat("Walking", 1);
        }
        //if nothing is pressed. Else it would move on and on
        if (!Input.GetKey("w") && !Input.GetKey("a") && !Input.GetKey("s") && !Input.GetKey("d"))
        {
            movementScript.direction = "";
            animator.SetBool("Run", false);
            animator.SetFloat("Walking", 0);
        }
        //Debug.Log(animator.GetBool("Run"));
    }
}
