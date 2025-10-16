using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class DungeonPlayerController : MonoBehaviour
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

 

 

    public Transform respawnPoint;
    public Animator anim;
    private Vector3 lastPosition;
    private bool isClimbing = false;
    private int currentSceneIndex;
    public bool oalaActivata = false;
    public GameObject mesajFinal; // Text TMP care va afișa "Ai Câștigat"
    private bool aCastigat = false;
    public GameObject oala;
    public Transform cameraPivot;
    public float mouseSensitivity = 2f;
    private float pitch = 0f;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        

       
        anim = GetComponentInChildren<Animator>();
        lastPosition = transform.position;
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (mesajFinal != null)
            mesajFinal.SetActive(false);

        GameObject oala = GameObject.Find("GoldPotRainbow");
        if (oala != null)
            oala.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (anim == null)
            return;

        if (aCastigat) return;

        if (isClimbing)
        {
            float vertical = Input.GetAxis("Vertical");
            transform.Translate(Vector3.up * vertical * moveSpeed * Time.deltaTime);
            return; // nu executa restul codului de mișcare
        }

        // Verificare sol
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Mișcare pe plan
        // Mișcare pe plan
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = (cameraTransform.right * x + cameraTransform.forward * z);
        move.y = 0f;
        controller.Move(move.normalized * moveSpeed * Time.deltaTime);

        // Rotație player pe Y doar după direcția de mișcare
        if (move.magnitude > 0.1f)
        {
            Vector3 flatDirection = new Vector3(move.x, 0f, move.z);
            Quaternion lookRotation = Quaternion.LookRotation(flatDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10f * Time.deltaTime);
        }

        // Sărit
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            anim.SetTrigger("Jump");
        }


        // Gravitație
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Respawn dacă cazi
        if (transform.position.y < -10f)
        {
            transform.position = respawnPoint.position;
            velocity = Vector3.zero;
        }
        Vector3 movementDelta = transform.position - lastPosition;
        float speed = new Vector3(movementDelta.x, 0, movementDelta.z).magnitude / Time.deltaTime;
        anim.SetFloat("Speed", speed);
        lastPosition = transform.position;

        Debug.Log("Speed: " + speed);
        // Control cameră sus-jos
        // Control cameră sus-jos
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -30f, 60f);

        if (cameraPivot != null)
            cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);

        Debug.Log("Mouse Y: " + Input.GetAxis("Mouse Y") + " | pitch: " + pitch);



    }

    private void OnTriggerEnter(Collider other)
    {

       
        


       
    }
    private System.Collections.IEnumerator ReturnToMenuAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(0); // încarcă scena cu index 0
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
