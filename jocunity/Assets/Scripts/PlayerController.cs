using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public Transform cameraTransform;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    public Transform groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundMask;

    public TMP_Text textAfisat;
    private int scor;

    public Transform respawnPoint;
    public Animator anim;
    private Vector3 lastPosition;
    private bool isClimbing = false;

    public GameObject witchObject; // model vrajitoare
    public CanvasGroup witchCanvasGroup; // pentru text
    public Image poofImage; // imaginea POOF!
    public CanvasGroup screenFadePanel; // optional - pentru fade la negru

    public Transform cameraPivot;
    public float mouseSensitivity = 2f;
    private float pitch = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        scor = 0;
        textAfisat.text = $"Scor: {scor}";

        anim = GetComponentInChildren<Animator>();
        lastPosition = transform.position;

        if (witchObject != null)
            witchObject.SetActive(false);

        if (witchCanvasGroup != null)
            witchCanvasGroup.gameObject.SetActive(false);

        if (poofImage != null)
            poofImage.gameObject.SetActive(false);

        if (screenFadePanel != null)
            screenFadePanel.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (isClimbing)
        {
            float vertical = Input.GetAxis("Vertical");
            transform.Translate(Vector3.up * vertical * moveSpeed * Time.deltaTime);
            return;
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = (cameraTransform.right * x + cameraTransform.forward * z);
        move.y = 0f;
        controller.Move(move.normalized * moveSpeed * Time.deltaTime);

        if (move.magnitude > 0.1f)
        {
            Vector3 flatDirection = new Vector3(move.x, 0f, move.z);
            Quaternion lookRotation = Quaternion.LookRotation(flatDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10f * Time.deltaTime);
        }

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            anim.SetTrigger("Jump");
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (transform.position.y < -10f)
        {
            transform.position = respawnPoint.position;
            velocity = Vector3.zero;
        }

        Vector3 movementDelta = transform.position - lastPosition;
        float speed = new Vector3(movementDelta.x, 0, movementDelta.z).magnitude / Time.deltaTime;
        anim.SetFloat("Speed", speed);
        lastPosition = transform.position;

        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -30f, 60f);

        if (cameraPivot != null)
            cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger atins cu: " + other.name);

        if (other.CompareTag("banuti"))
        {
            other.gameObject.SetActive(false);
            PlaySound("Sunet");

            scor += 10;
            textAfisat.text = $"Scor: {scor}";
        }

        if (other.CompareTag("oala"))
        {
            other.gameObject.SetActive(false);
            PlaySound("Sunet");

            if (witchObject != null || witchCanvasGroup != null)
            {
                StartCoroutine(FadeInWitchCombined());
            }
        }

        if (other.CompareTag("Ladder"))
        {
            isClimbing = true;
            controller.enabled = false;
            anim.SetBool("isClimbing", true);
        }
    }

    private IEnumerator FadeInWitchCombined()
    {
        Debug.Log("FadeInWitchCombined STARTED");

        if (witchObject != null)
            witchObject.SetActive(true);

        if (witchCanvasGroup != null)
        {
            Debug.Log("witchCanvasGroup FOUND → Activating!");
            witchCanvasGroup.gameObject.SetActive(true);
            witchCanvasGroup.alpha = 0f;
        }

        Renderer renderer = witchObject != null ? witchObject.GetComponentInChildren<Renderer>() : null;
        Material[] mats = null;

        if (renderer != null)
        {
            mats = renderer.materials;
            foreach (var mat in mats)
            {
                Color color = mat.GetColor("_Color");
                color.a = 0f;
                mat.SetColor("_Color", color);
            }
        }

        float duration = 1.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsed / duration);

            if (mats != null)
            {
                foreach (var mat in mats)
                {
                    Color color = mat.GetColor("_Color");
                    color.a = alpha;
                    mat.SetColor("_Color", color);
                }
            }

            if (witchCanvasGroup != null)
                witchCanvasGroup.alpha = alpha;

            yield return null;
        }

        if (mats != null)
        {
            foreach (var mat in mats)
            {
                Color color = mat.GetColor("_Color");
                color.a = 1f;
                mat.SetColor("_Color", color);
            }
        }

        if (witchCanvasGroup != null)
            witchCanvasGroup.alpha = 1f;

       
        yield return new WaitForSeconds(3f);

        

        if (poofImage != null)
        {
            poofImage.gameObject.SetActive(true);

            Color color = poofImage.color;
            color.a = 0f;
            poofImage.color = color;

            float poofFadeDuration = 0.8f;
            float poofElapsed = 0f;

            while (poofElapsed < poofFadeDuration)
            {
                poofElapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(0f, 1f, poofElapsed / poofFadeDuration);
                color.a = alpha;
                poofImage.color = color;
                yield return null;
            }

            color.a = 1f;
            poofImage.color = color;

            yield return new WaitForSeconds(2f);
        }

        Debug.Log("LOADING NEXT SCENE...");

        SceneManager.LoadScene("Scena2"); 
    }

    void OnTriggerExit(Collider other)
    {
        if (isClimbing && other.CompareTag("Ladder"))
        {
            isClimbing = false;
            controller.enabled = true;
            anim.SetBool("isClimbing", false);
        }
    }
    public int GetScore()
    {
        return scor;
    }

    public void ForceExitLadder()
    {
        isClimbing = false;
        controller.enabled = true;
        anim.SetBool("isClimbing", false);
    }

    private void PlaySound(string tag)
    {
        GameObject audioObject = GameObject.FindWithTag(tag);
        if (audioObject != null)
        {
            AudioSource audioSource = audioObject.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.Play();
            }
        }
    }
}
