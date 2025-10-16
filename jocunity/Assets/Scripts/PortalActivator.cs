using UnityEngine;

public class PortalActivator : MonoBehaviour
{
    public int targetScore = 40;
    public GameObject portalObject; // tragi aici portalul (inactive la început)
    public PlayerController playerController; // tragi PlayerController (unde ai scor-ul)

    private bool portalActivated = false;

    void Update()
    {
        if (!portalActivated && playerController != null && playerController.GetScore() >= targetScore)
        {
            // Activăm portalul
            portalObject.SetActive(true);
            portalActivated = true;
            Debug.Log("Portal ACTIVAT — scor atins!");
        }
    }
}
