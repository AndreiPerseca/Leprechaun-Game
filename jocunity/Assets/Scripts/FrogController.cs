using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;

    [Header("Jump")]
    public float baseJumpHeight = 2f;
    public float maxPower = 3f;
    public float chargeSpeed = 1f;

    private float powerJumpCharge = 0f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundMask;

    private Animator anim;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Rotate to movement direction
        if (move.magnitude > 0.1f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10f * Time.deltaTime);
        }

        // Set "Speed" param for walk anim (if needed)
        anim.SetFloat("Speed", move.magnitude);

        // Power Jump
        if (isGrounded && Input.GetButton("Jump"))
        {
            powerJumpCharge += chargeSpeed * Time.deltaTime;
            powerJumpCharge = Mathf.Clamp(powerJumpCharge, 0f, maxPower);

            // Optional: anim.SetFloat("Charge", powerJumpCharge / maxPower);
        }

        if (isGrounded && Input.GetButtonUp("Jump"))
        {
            float jumpPower = baseJumpHeight + powerJumpCharge;
            velocity.y = Mathf.Sqrt(jumpPower * -2f * gravity);

            anim.SetTrigger("Jump");

            powerJumpCharge = 0f;
        }

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
