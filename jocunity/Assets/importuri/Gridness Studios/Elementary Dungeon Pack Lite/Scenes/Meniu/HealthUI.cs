using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthUI : MonoBehaviour
{
    public Image[] hearts;
    public Sprite heartFull;
    public Sprite heartEmpty;
    public GameObject gameOverText;
    public GameObject respawnButton;

    public MonoBehaviour movementScript;  // 🔧 referință la PlayerController (doar scriptul)

    private int currentHealth = 3;

    void Start()
    {
        if (gameOverText != null) gameOverText.SetActive(false);
        if (respawnButton != null) respawnButton.SetActive(false);

        // 🔒 La start → mouse blocat pentru gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ApplyDamage()
    {
        if (currentHealth <= 0) return;

        currentHealth--;
        UpdateHearts();

        if (currentHealth <= 0)
        {
            ShowGameOver();
        }
    }

    private void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = i < currentHealth ? heartFull : heartEmpty;
        }
    }

    private void ShowGameOver()
    {
        if (gameOverText != null) gameOverText.SetActive(true);
        if (respawnButton != null) respawnButton.SetActive(true);

        if (movementScript != null)
        {
            movementScript.enabled = false; // 🔒 dezactivează doar scriptul de control
        }

        // 🎮 🔓 Mouse devine USEABLE
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OnRespawnButton()
    {
        Debug.Log("Respawn pressed!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
