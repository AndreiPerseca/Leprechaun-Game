// 5/23/2025 AI-Tag
// This was created with assistance from Muse, a Unity Artificial Intelligence product

using UnityEngine;

public class PickableObject : MonoBehaviour
{
    public bool isPickedUp = false; // Indică dacă obiectul este deja ridicat
    public void PickUp(Transform pickUpPoint)
{
    isPickedUp = true;
    gameObject.isStatic = false;

    // Setăm părintele
    transform.SetParent(pickUpPoint);

    // Poziționează exact pivotul obiectului pe PickupPoint
    transform.localPosition = Vector3.zero;

    // Resetăm rotația locală
    transform.localRotation = Quaternion.identity;

    // Dezactivăm fizica
    Rigidbody rb = GetComponent<Rigidbody>();
    if (rb != null)
    {
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    Debug.Log("Picked up (pivot-centered): " + gameObject.name);
}





    public void Drop()
    {
        isPickedUp = false;
        transform.SetParent(null); // Dezatașează obiectul de PickupPoint
        GetComponent<Rigidbody>().isKinematic = false; // Reactivare fizică
    }
}