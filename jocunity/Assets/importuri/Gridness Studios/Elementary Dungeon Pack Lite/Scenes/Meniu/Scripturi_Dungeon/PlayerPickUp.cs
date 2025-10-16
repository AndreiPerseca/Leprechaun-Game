// 5/23/2025 AI-Tag
// This was created with assistance from Muse, a Unity Artificial Intelligence product

using UnityEngine;

public class PlayerPickUp : MonoBehaviour
{
    public Transform pickUpPoint; // Referință către obiectul PickUpPoint al playerului
    public float pickUpRange = 2.0f; // Distanța până la care poate ridica obiecte
    private PickableObject pickedObject = null; // Obiectul curent ridicat

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Tasta pentru ridicare/lăsare obiect
        {
            if (pickedObject == null) // Dacă nu ai ridicat deja un obiect
            {
                TryPickUp();
            }
            else // Dacă deja ții un obiect, îl lași jos
            {
                DropObject();
            }
        }
    }

    void TryPickUp()
    {
        // Creează o rază din poziția playerului
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, pickUpRange))
        {
            PickableObject pickable = hit.transform.GetComponent<PickableObject>();

            if (pickable != null && !pickable.isPickedUp)
            {
                pickedObject = pickable; // Setează obiectul ca fiind ridicat
                pickedObject.PickUp(pickUpPoint); // Execută logica de ridicare
            }
        }
    }

    void DropObject()
    {
        if (pickedObject != null)
        {
            pickedObject.Drop(); // Execută logica de lăsare a obiectului
            pickedObject = null; // Golește referința la obiectul ridicat
        }
    }
}