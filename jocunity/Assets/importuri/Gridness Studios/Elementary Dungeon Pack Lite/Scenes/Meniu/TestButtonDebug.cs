using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonControllerr : MonoBehaviour
{
    public void ReloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}
