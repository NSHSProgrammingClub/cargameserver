using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private CharacterController charController;

    [SerializeField] private float speed;

    [SerializeField] private float jumpMagnitude;

    [SerializeField] private float mass = 1f;

    private float yVel;


    private Vector3 movement = Vector3.zero;
    private Vector3 jump;

    #region Collision Check Vars
    
    [SerializeField] private Transform groundCheck;
    private float groundDistance;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private float slideFriction;
    private Vector3 hitNormal;

    [SerializeField] private CapsuleCollider groundCollider;

    #endregion

    #region Caching

    private float originalSlopeLimit;

    #endregion

    private bool isJump;
    // might not need this
    private bool isGrounded => Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);
    //private bool isGrounded;

    private void Start()
    {
        charController = GetComponent<CharacterController>();

        jump = Vector3.up * jumpMagnitude;

        groundDistance = charController.skinWidth;

        originalSlopeLimit = charController.slopeLimit;
    }

    private void GetInput()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        movement = (transform.right * x + transform.forward * z).normalized;

        isJump = Input.GetButtonDown("Jump");

    }

    private void Jump()
    {
        if (isJump && charController.isGrounded)
        {
            //Debug.Log("jumping");
            yVel = Mathf.Sqrt(jumpMagnitude * -2f * Physics.gravity.y);
            charController.Move(transform.up * yVel * Time.deltaTime);
            isJump = false;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitNormal = hit.normal;
    }

    private void DoGravity()
    {

        yVel += Physics.gravity.y * mass * Time.deltaTime;


        charController.Move(transform.up * yVel * Time.deltaTime);
    }

    private void Move() => CmdMove();
    
    private void CmdMove() => RpcMove();

    private void RpcMove() => charController.Move(movement * speed * Time.deltaTime);



    private void Update()
    {
        if (charController.isGrounded && movement.y == 0)
            yVel = -2f;


        if (charController.isGrounded)
            charController.slopeLimit = originalSlopeLimit;
        else
            charController.slopeLimit = 90f;


        if (!GameManager.isInMenu)
        {
            GetInput();
            Move();
            DoGravity();
            Jump();
        }
    }

    private void FixedUpdate()
    {
    }
}

