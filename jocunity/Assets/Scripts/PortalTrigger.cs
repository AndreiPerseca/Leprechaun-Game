using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalTrigger : MonoBehaviour
{
    public string sceneToLoad = "Scena3";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // player trebuie să aibă tag Player
        {
            Debug.Log("Player a intrat în portal → schimbăm scena...");
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
