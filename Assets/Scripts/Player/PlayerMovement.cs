using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{

    private CharacterController characterController;

    [SerializeField] private float playerSpeed=120;
    [SerializeField] private float playerJumpMagnitude=25;

    private Vector3 velocity = Vector3.zero;

    private bool isJumping;
    private Vector3 xzMovement;

    private float originalSlopeLimit;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        originalSlopeLimit = characterController.slopeLimit;
    }

    private void GetInput()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        xzMovement = (transform.right * x + transform.forward * z).normalized * playerSpeed;
        if (Input.GetButtonDown("Jump")) isJumping = true;
    }

    private void Jump()
    {
        if (isJumping) {
            //Debug.Log("jump! "+characterController.isGrounded+" velocity.y "+velocity.y);
            if (characterController.isGrounded)
            {

                float yJerk = Mathf.Sqrt(playerJumpMagnitude * -2f * Physics.gravity.y);//Magic code by Sasha
                velocity.y = yJerk;
            }
            isJumping = false;
        }
    }


    private void DoGravity()
    {

        //yVel += Physics.gravity.y * mass * Time.deltaTime;
        velocity.y += Physics.gravity.y* Time.deltaTime*3;
        //characterController.Move(transform.up * yVel * Time.deltaTime);
    }



    private void Update()
    {


        if (characterController.isGrounded)

            characterController.slopeLimit = originalSlopeLimit;
        else
            characterController.slopeLimit = 90f;



        GetInput();
        DoGravity();
        Jump();


        velocity += xzMovement * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);
        if(Time.deltaTime!=0){
            velocity.x = characterController.velocity.x ;
            velocity.z = characterController.velocity.z ;
        }
        /*
        Make sure y velocity is not too far from real y velocity. (y vel still has to be less than real y vel for ground collition to work)
        */
        if(characterController.velocity.y-2>velocity.y){
            velocity.y=Mathf.Max(velocity.y,-2);
        }
        if(characterController.velocity.y<velocity.y){
            velocity.y=characterController.velocity.y;
        }
    }

    private void  FixedUpdate()
    {
        velocity.x *= .8f;
        velocity.y *= .99f;
        velocity.z *= .8f;

    }
}
