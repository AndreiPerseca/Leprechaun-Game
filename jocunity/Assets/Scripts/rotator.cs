using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0f, 100f, 0f); // grade pe secundă

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
