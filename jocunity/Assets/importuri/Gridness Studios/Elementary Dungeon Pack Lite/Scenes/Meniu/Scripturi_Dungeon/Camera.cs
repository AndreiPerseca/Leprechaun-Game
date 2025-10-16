using UnityEngine;

public class DungeonCameraController : MonoBehaviour
{
    public Transform target;
    public float distance = 5f;

    public float mouseSensitivityX = 2f;
    public float mouseSensitivityY = 2f;

    public float minY = -20f;
    public float maxY = 80f;

    public float scrollSensitivity = 2f;
    public float minDistance = 3f;
    public float maxDistance = 10f;

    private float currentYaw = 0f;
    private float currentPitch = 20f;

    public bool cameraLocked = false;
    public bool overrideLookAt = false;
    public Transform overrideTarget;

    void Start()
    {
        if (target == null)
        {
            Debug.LogWarning("Camera target NU este setat! (DungeonCameraController)");
        }
    }

    void LateUpdate()
    {
        if (cameraLocked)
        {
            return;
        }

        // Mouse look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivityX;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivityY;

        currentYaw += mouseX;
        currentPitch -= mouseY;
        currentPitch = Mathf.Clamp(currentPitch, minY, maxY);

        // Zoom cu scroll
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distance -= scroll * scrollSensitivity;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        // Calculăm poziția camerei
        Vector3 direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);

        transform.position = target.position + rotation * direction;

        // LookAt
        if (overrideLookAt && overrideTarget != null)
        {
            transform.LookAt(overrideTarget.position);
        }
        else
        {
            transform.LookAt(target.position);
        }
    }

    public void ResetCameraAngles()
    {
        currentYaw = 0f;
        currentPitch = 20f; // default Yaw/Pitch pentru gameplay
    }
}
