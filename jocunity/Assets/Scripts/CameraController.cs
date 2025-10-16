using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // referință la jucător
    public float distance = 5.0f;
    public float mouseSensitivityX = 5.0f;
    public float mouseSensitivityY = 1.0f;
    public float minY = -30f;
    public float maxY = 60f;

    private float currentYaw = 0f;
    private float currentPitch = 0f;

    void Start()
    {
       
    }

    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivityX;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivityY;

        currentYaw += mouseX;
        currentPitch -= mouseY;
        currentPitch = Mathf.Clamp(currentPitch, minY, maxY);

        Vector3 direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
        transform.position = target.position + rotation * direction;

        transform.LookAt(target.position);
    }
}

