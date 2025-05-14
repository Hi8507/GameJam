using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            // Optional: Check tag or other conditions if you don't want to rotate everything
            // if (other.CompareTag("Player") || other.CompareTag("Rotatable"))

            // Rotate the entering object to 0, 90, 0
            other.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
    }
}
