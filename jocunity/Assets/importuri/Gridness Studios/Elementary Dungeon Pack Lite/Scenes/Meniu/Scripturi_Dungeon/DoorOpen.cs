using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityObjectSwitcher : MonoBehaviour
{
    public GameObject objectClosed;   // Ușa închisă
    public GameObject objectOpened;   // Ușa deschisă
    public GameObject[] requiredObjects; // Obiectele necesare (care au PickableObject)
    public float checkRadius = 2f;       // Raza în care verificăm
    private bool isOpen = false;

    void Update()
    {
        bool shouldOpen = AreAllRequiredObjectsNearby();

        // Dacă starea se schimbă (ex: era închisă și trebuie deschisă)
        if (shouldOpen != isOpen)
        {
            isOpen = shouldOpen;
            ToggleMeshComponents(objectClosed, !isOpen);
            ToggleMeshComponents(objectOpened, isOpen);
        }
    }

    private bool AreAllRequiredObjectsNearby()
    {
        foreach (GameObject obj in requiredObjects)
        {
            if (obj == null || obj.GetComponent<PickableObject>() == null)
                return false;

            float dist = Vector3.Distance(obj.transform.position, transform.position);
            if (dist > checkRadius)
                return false;
        }
        return true;
    }

    private void ToggleMeshComponents(GameObject obj, bool isActive)
    {
        if (obj != null)
        {
            MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
                meshRenderer.enabled = isActive;

            MeshCollider meshCollider = obj.GetComponent<MeshCollider>();
            if (meshCollider != null)
                meshCollider.enabled = isActive;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}

