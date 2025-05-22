using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterHeartrate : MonoBehaviour
{

    public AudioSource Heartrate;

    public bool SanityOpen=false;  
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            Heartrate.Play();
            SanityOpen = true;
            
        }

    }

    public void OnTriggerExit(Collider other)
    {
        Heartrate.Stop();
        SanityOpen=false;
    }
}
