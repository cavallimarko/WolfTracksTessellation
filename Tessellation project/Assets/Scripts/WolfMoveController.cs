using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfMoveController : MonoBehaviour
{
    public float movementSpeed=2;
    public float currentSpeeed = 0;
    private float speedVelocity = 0;
    private float speedTime = 0.1f;
    public float rotationSpeed = 0.1f;
    private float gravity = 3f;

    private Transform mainCamera;

    private CharacterController controller;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        mainCamera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Move();   
    }
    private void Move()
    {
        Vector2 movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector3 forward = mainCamera.forward;
        Vector3 right = mainCamera.right;
        forward.Normalize();
        right.Normalize();
        Vector3 desiredMoveDirection = (forward * movementInput.y + right * movementInput.x).normalized;
        Vector3 gravityVector = Vector3.zero;
        if (!controller.isGrounded)
        {
            gravityVector.y -= gravity;
        }
        if (desiredMoveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(desiredMoveDirection),rotationSpeed);
        }
        float multiplier = 0.5f;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            multiplier = 2;
        }
        float targetSpeed = movementSpeed * movementInput.magnitude*multiplier;
        currentSpeeed = Mathf.SmoothDamp(currentSpeeed,targetSpeed,ref speedVelocity,speedTime);
        
        controller.Move(gravityVector * Time.deltaTime);
        
        
        controller.Move(desiredMoveDirection * currentSpeeed * Time.deltaTime);
        animator.SetFloat("MovementSpeed", multiplier * movementInput.normalized.magnitude, speedTime, Time.deltaTime);
    }
}
