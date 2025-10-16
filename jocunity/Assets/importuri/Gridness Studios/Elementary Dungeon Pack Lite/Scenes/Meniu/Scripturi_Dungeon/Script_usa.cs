// 5/22/2025 AI-Tag
// This was created with assistance from Muse, a Unity Artificial Intelligence product

using UnityEngine;

public class ObjectSwitcherWithMeshComponents : MonoBehaviour
{
    public GameObject object1; // Obiectul care începe activ
    public GameObject object2; // Obiectul care începe inactiv
    public Transform player;  // Transform-ul jucătorului
    public Transform interactableObject; // Obiectul cu care jucătorul interacționează

    public float interactionRange = 3.0f; // Distanța maximă pentru interacțiune
    private bool isObject1Active = true;  // Reține care obiect este activ

    void Update()
    {
        // Calculăm distanța dintre jucător și obiectul interactiv
        float distance = Vector3.Distance(player.position, interactableObject.position);

        // Verificăm dacă jucătorul este în raza de interacțiune
        if (distance <= interactionRange)
        {
            // Detectăm dacă s-a apăsat tasta E
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Comutăm starea obiectelor
                isObject1Active = !isObject1Active;

                // Actualizăm Mesh Renderer și Mesh Collider pentru fiecare obiect
                ToggleMeshComponents(object1, isObject1Active);
                ToggleMeshComponents(object2, !isObject1Active);
            }
        }
    }

    private void ToggleMeshComponents(GameObject obj, bool isActive)
    {
        if (obj != null)
        {
            // Activăm/dezactivăm Mesh Renderer
            MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.enabled = isActive;
            }

            // Activăm/dezactivăm Mesh Collider
            MeshCollider meshCollider = obj.GetComponent<MeshCollider>();
            if (meshCollider != null)
            {
                meshCollider.enabled = isActive;
            }
        }
    }

    // Vizualizăm raza de interacțiune în Scene View
    private void OnDrawGizmosSelected()
    {
        if (interactableObject != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(interactableObject.position, interactionRange);
        }
    }
}