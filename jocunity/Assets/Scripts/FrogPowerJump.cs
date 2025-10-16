using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogPowerJump : MonoBehaviour
{
    private CharacterController controller;
    private Animator anim;
    private Vector3 velocity;
    private bool isGrounded;

    [Header("Jump Settings")]
    public float gravity = -9.81f;
    public float normalJumpHeight = 2f;
    public float basePowerJumpHeight = 2f;
    public float maxPower = 3f;
    public float chargeSpeed = 1f;
    public float chargeThreshold = 0.2f; // timp minim pt PowerJump

    private float powerJumpCharge = 0f;
    private float jumpButtonHoldTime = 0f;
    private bool isHoldingJump = false;
    private bool isCharging = false;

    private bool forcePowerJump = false;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundMask;

    [Header("Respawn")]
    public Transform respawnPoint; // LEGĂM în Inspector (poți trage "respawn" acolo)

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // Check if grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // NORMAL Jump → pe buton Jump
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(normalJumpHeight * -2f * gravity);
            anim.SetTrigger("Jump");
            Debug.Log("Normal Jump performed.");
        }

        // POWER Jump → pe ALT BUTON (ex: LeftShift / Fire2)
        bool powerJumpButtonDown = Input.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.LeftShift);
        bool powerJumpButtonHold = Input.GetButton("Fire2") || Input.GetKey(KeyCode.LeftShift);
        bool powerJumpButtonUp = Input.GetButtonUp("Fire2") || Input.GetKeyUp(KeyCode.LeftShift);

        // START Power Jump input
        if (isGrounded && powerJumpButtonDown)
        {
            isHoldingJump = true;
            jumpButtonHoldTime = 0f;
            powerJumpCharge = 0f;
            isCharging = false;
            forcePowerJump = false;

            anim.SetFloat("Charge", 0f);
            Debug.Log("PowerJump button DOWN → charging...");
        }

        // HOLD Power Jump input
        if (isGrounded && powerJumpButtonHold && isHoldingJump)
        {
            jumpButtonHoldTime += Time.deltaTime;

            if (jumpButtonHoldTime >= chargeThreshold && !forcePowerJump)
            {
                // Prima dată când trec pragul → charging activ
                forcePowerJump = true;
                isCharging = true;

                Debug.Log(">>> ENTERED CHARGE MODE (PowerJump)");
            }

            if (isCharging)
            {
                powerJumpCharge += chargeSpeed * Time.deltaTime;
                powerJumpCharge = Mathf.Clamp(powerJumpCharge, 0f, maxPower);

                anim.SetFloat("Charge", powerJumpCharge / maxPower);

                Debug.Log("Charging... power = " + powerJumpCharge);
            }
            else
            {
                anim.SetFloat("Charge", 0f);
            }
        }

        // RELEASE Power Jump input
        if (isGrounded && powerJumpButtonUp && isHoldingJump)
        {
            if (forcePowerJump)
            {
                // Power Jump
                float jumpPower = basePowerJumpHeight + powerJumpCharge;
                velocity.y = Mathf.Sqrt(jumpPower * -2f * gravity);
                anim.SetTrigger("Jump"); // sau "PowerJump"
                Debug.Log("Power Jump executed!");
            }
            else
            {
                // Nu a ținut destul → nimic
                Debug.Log("Released too early → No Power Jump.");
            }

            // Reset state
            isHoldingJump = false;
            isCharging = false;
            jumpButtonHoldTime = 0f;
            powerJumpCharge = 0f;
            forcePowerJump = false;

            anim.SetFloat("Charge", 0f);
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // ✅ RESPAWN logic → ACUM în FrogPowerJump → CORECT!
        if (transform.position.y < -10f && respawnPoint != null)
        {
            transform.position = respawnPoint.position;
            velocity = Vector3.zero;
            Debug.Log("RESPAWNED FROG!");
        }
    }

    // Public accessor (optional)
    public bool IsHoldingJump()
    {
        return isHoldingJump;
    }
}
