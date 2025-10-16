using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManMenuScript : MonoBehaviour
{
    
    public GameObject panelInstruct;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        panelInstruct.SetActive(false);
        
    }

    public void LanseazaScenaDinButon(int indexScena)
    {
        SceneManager.LoadScene(indexScena);
    }

    public void Back() 
    {
        panelInstruct.SetActive(false);
    }

    public void LoadPanelInstruct()
    {
        panelInstruct.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}

