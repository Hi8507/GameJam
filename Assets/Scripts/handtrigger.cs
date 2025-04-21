using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handtrigger : MonoBehaviour
{
    public int Sanitylose = 15;
    public int SanityPoints = 0;
    // Start is called before the first frame update
    void Start()
    {
        SanityPoints = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            SanityPoints = -Sanitylose;
        }
    }
}
