using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WitchDeathNotifier : MonoBehaviour
{
    public GameObject targetObject;    // Obiectul pe care îl urmărim (ex: "default")
    public GameObject victoryPanel;    // Panel-ul de Victory

    private bool victoryTriggered = false; // Ca să nu pornim de mai multe ori

    void Update()
    {
        // DACA obiectul a fost DISTRUS (Destroy), targetObject == null
        if (!victoryTriggered && targetObject == null)
        {
            Debug.Log("Target object a fost distrus! Afisam VictoryPanel.");
            victoryTriggered = true;

            if (victoryPanel != null)
            {
                victoryPanel.SetActive(true); // Afisam panelul
                StartCoroutine(LoadFirstSceneAfterDelay(5f)); // Timer 5 sec
            }
        }
    }

    private IEnumerator LoadFirstSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Dupa 5 sec → log + schimbare scena
        Debug.Log("Trecem la prima scena...");
        SceneManager.LoadScene(0); // Index 0 = menu
    }
}
