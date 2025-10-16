using UnityEngine;

public class PlayerTransformTrigger : MonoBehaviour
{
    public GameObject uiPanel; // tragi aici panelul cu butoane (PlayerTransformPanel)

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Player (Broasca) → trebuie să aibă tag Player
        {
            Debug.Log("Player a intrat în trigger → Afișăm UI pentru transformare.");
            uiPanel.SetActive(true);

            // OPTIONAL → dacă vrei să oprești jocul în spate:
            // Time.timeScale = 0f;
        }
    }
}
