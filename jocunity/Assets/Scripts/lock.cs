using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lockasd : MonoBehaviour
{
 private Quaternion originalRotation;
// Start is called before the first frame update
void Start()
    {
        originalRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = originalRotation;
    }
}
