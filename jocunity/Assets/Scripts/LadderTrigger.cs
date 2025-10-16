using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderTrigger : MonoBehaviour
{
    public PlayerController playerController;
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            playerController.ForceExitLadder();
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
