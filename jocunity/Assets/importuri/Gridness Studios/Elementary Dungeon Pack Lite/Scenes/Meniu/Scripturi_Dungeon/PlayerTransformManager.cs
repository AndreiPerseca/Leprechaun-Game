using UnityEngine;
using System.Collections;

public class PlayerTransformManager : MonoBehaviour
{
    public GameObject broascaObject;
    public GameObject model1Object;
    public GameObject model2Object;
    public GameObject model3Object;

    public Camera mainCamera;
    public float defaultFOV = 60f;
    public float zoomDuration = 0.5f;

    public DungeonCameraController cameraFollowScript;

    // Referință către controllerul de mișcare și atac
    public DungeonPlayerController playerController;
    public PlayerAttack playerAttack;

    public void SelectModel1()
    {
        ActivateModel(1);
    }

    public void SelectModel2()
    {
        ActivateModel(2);
    }

    public void SelectModel3()
    {
        ActivateModel(3);
    }

    private void ActivateModel(int modelIndex)
    {
        Debug.Log("Activăm modelul " + modelIndex);

        // Dezactivăm toate modelele
        broascaObject.SetActive(false);
        model1Object.SetActive(false);
        model2Object.SetActive(false);
        model3Object.SetActive(false);

        // Activăm doar modelul selectat
        if (modelIndex == 1) model1Object.SetActive(true);
        else if (modelIndex == 2) model2Object.SetActive(true);
        else if (modelIndex == 3) model3Object.SetActive(true);

        // Găsim Animatorul de pe modelul activ
        Animator newAnimator = null;

        if (modelIndex == 1 && model1Object != null)
            newAnimator = model1Object.GetComponent<Animator>();
        else if (modelIndex == 2 && model2Object != null)
            newAnimator = model2Object.GetComponent<Animator>();
        else if (modelIndex == 3 && model3Object != null)
            newAnimator = model3Object.GetComponent<Animator>();

        // Setăm Animatorul în ambele scripturi
        if (newAnimator != null)
        {
            if (playerController != null)
            {
                playerController.anim = newAnimator;
                Debug.Log("Animator setat în DungeonPlayerController.");
            }

            if (playerAttack != null)
            {
                playerAttack.animator = newAnimator;
                Debug.Log("Animator setat în PlayerAttack.");
            }
        }

        // Resetare cameră
        cameraFollowScript.cameraLocked = false;
        cameraFollowScript.overrideLookAt = false;
        cameraFollowScript.overrideTarget = null;
        cameraFollowScript.ResetCameraAngles();

        StartCoroutine(ZoomCameraOut());

        gameObject.SetActive(false); // Ascundem UI
        CursorUtils.SetCursorState(true);
    }

    private IEnumerator ZoomCameraOut()
    {
        float startFOV = mainCamera.fieldOfView;
        float timer = 0f;

        while (timer < zoomDuration)
        {
            timer += Time.deltaTime;
            mainCamera.fieldOfView = Mathf.Lerp(startFOV, defaultFOV, timer / zoomDuration);
            yield return null;
        }

        mainCamera.fieldOfView = defaultFOV;
    }

    public void DisablePlayerControl()
    {
        var controller = GetComponent<DungeonPlayerController>();
        if (controller != null) controller.enabled = false;

        var attack = GetComponent<PlayerAttack>();
        if (attack != null) attack.enabled = false;

        var pickup = GetComponent<PlayerPickUp>();
        if (pickup != null) pickup.enabled = false;

        Debug.Log("Player control DISABLED.");
    }

    public void EnablePlayerControl()
    {
        var controller = GetComponent<DungeonPlayerController>();
        if (controller != null) controller.enabled = true;

        var attack = GetComponent<PlayerAttack>();
        if (attack != null) attack.enabled = true;

        var pickup = GetComponent<PlayerPickUp>();
        if (pickup != null) pickup.enabled = true;

        Debug.Log("Player control ENABLED.");
    }
}