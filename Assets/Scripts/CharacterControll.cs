using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControll : MonoBehaviour
{
    Animator playerAnimations;
    Rigidbody AJrigidbody;
    CharacterHitCollider hitCollider;
    Vector3 moveDirection = Vector3.zero;
    bool walkState;
    float sprint;
    float sprintCost;

    [SerializeField]
    private float movement;
    [SerializeField]
    private float sensitivity;
    [SerializeField]
    private float jumpForce;

    void Start()
    {
        walkState = false;
        playerAnimations = this.GetComponent<Animator>() as Animator;
        AJrigidbody = this.GetComponent<Rigidbody>() as Rigidbody;
        hitCollider = this.GetComponentInChildren<CharacterHitCollider>() as CharacterHitCollider;
        sprint = 100.0f;
        sprintCost = 25.0f; // 100/10 means 10s of extra speed
    }

    void Update()
    {
        this.transform.eulerAngles -= new Vector3(Input.GetAxis("Mouse Y") * sensitivity, -Input.GetAxis("Mouse X") * sensitivity, 0);
        walkState = false;

        if (Input.GetMouseButtonDown(0))
            CatchControl();

        if (Input.GetKey(KeyCode.W))
            walkState = Walk(transform.forward);

        if (Input.GetKey(KeyCode.S))
            walkState = Walk(-transform.forward);

        if (Input.GetKey(KeyCode.A))
            walkState = Walk(-transform.right);

        if (Input.GetKey(KeyCode.D))
            walkState = Walk(transform.right);

        if (!walkState)
            playerAnimations.SetBool("isWalking", false);

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

    }

    bool Walk(Vector3 direction)
    {
        float currentMovement = movement;
        float acceleration = 3;

        if (Input.GetKey(KeyCode.LeftShift) && direction == transform.forward) /* Run only forward */
        {
            if (sprint > 0)
            {
                playerAnimations.SetBool("isRunning", true);
                currentMovement = movement * acceleration;
                sprint -= sprintCost * Time.deltaTime;
            }
            else
            {
                sprint = 0f;
                playerAnimations.SetBool("isRunning", false);
            }
        }
        else
        {
            if (sprint < 100.0f)
                sprint += sprintCost * Time.deltaTime;
            else
                sprint = 100.0f;
            playerAnimations.SetBool("isRunning", false);
        }


        transform.position += direction * currentMovement * Time.deltaTime;
        playerAnimations.SetBool("isWalking", true);

        return true;
    }

    void Jump()
    {
        if (!playerAnimations.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            playerAnimations.SetTrigger("Jump");
            AJrigidbody.AddForce(transform.up * jumpForce);
        }
    }

    void CatchControl()
    {
        if(!playerAnimations.GetCurrentAnimatorStateInfo(0).IsName("Catch"))
        {
            playerAnimations.SetTrigger("Catch");
            hitCollider.TryCatch();
        }
    }

    public float GetSprint()
    {
        return sprint / 100.0f;
    }
}
