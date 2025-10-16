using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Tooltip("Cât de repede se rotește camera în sus/jos.")]
    public float sensitivity = 2f;

    [Tooltip("Limita minimă a unghiului (privirea în jos).")]
    public float minPitch = -30f;

    [Tooltip("Limita maximă a unghiului (privirea în sus).")]
    public float maxPitch = 60f;

    private float pitch = 0f;

    void Update()
    {
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        Debug.Log("Mouse Y: " + Input.GetAxis("Mouse Y") + " | Pitch: " + pitch);

    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

}

