using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handtrigger : MonoBehaviour
{
    public float Sanitylose = 15;
    public float SanityPoints = 0;


    // Start is called before the first frame update
    public void Start()
    {
        SanityPoints = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            
            SanityPoints -= Sanitylose;
        }
    }
}
