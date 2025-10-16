using UnityEngine;
using System.Collections;

public class CauldronInteraction : MonoBehaviour
{
    public GameObject uiPanel;
    public GameObject interactionPrompt;
    public PlayerTransformManager playerTransformManager;

    public Camera mainCamera;
    public Transform cauldronFocusPoint;
    public float zoomFOV = 30f;
    public float zoomDuration = 0.5f;

    public DungeonCameraController cameraFollowScript;

    private bool isPlayerNearby = false;
    private bool isZooming = false;

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Interacțiune cu Cauldron → Afișăm UI.");

            uiPanel.SetActive(true);

            playerTransformManager.DisablePlayerControl();

            cameraFollowScript.cameraLocked = true;
            cameraFollowScript.overrideLookAt = true;
            cameraFollowScript.overrideTarget = cauldronFocusPoint;

            if (!isZooming)
            {
                StartCoroutine(ZoomCameraIn());
            }

            CursorUtils.SetCursorState(false);

            if (interactionPrompt != null)
                interactionPrompt.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;

            if (interactionPrompt != null)
                interactionPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;

            if (interactionPrompt != null)
                interactionPrompt.SetActive(false);
        }
    }

    private IEnumerator ZoomCameraIn()
    {
        isZooming = true;

        float startFOV = mainCamera.fieldOfView;
        float timer = 0f;

        while (timer < zoomDuration)
        {
            timer += Time.deltaTime;
            mainCamera.fieldOfView = Mathf.Lerp(startFOV, zoomFOV, timer / zoomDuration);
            mainCamera.transform.LookAt(cauldronFocusPoint.position);
            yield return null;
        }

        mainCamera.fieldOfView = zoomFOV;
        mainCamera.transform.LookAt(cauldronFocusPoint.position);

        isZooming = false;
    }
}
