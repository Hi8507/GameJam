using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;

public class TriggerPills : MonoBehaviour
{

    public GameObject Takepills;

    public int SanityAdd = 20;
    public int SanityPoints=0;
    public bool yellow=false;
    public bool pink = false;
    public bool purple = false;
    bool asd1=false;
    // Start is called before the first frame update
    void Start()
    {
        Takepills.SetActive(false);
        asd1 = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            if (SanityPoints < 1)
            {
                Takepills.SetActive(true);
            }
        }
        }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            if (asd1 == false) { 
                if (Input.GetKeyDown(KeyCode.E))
                {
                    SanityPoints += SanityAdd;
                    Takepills.SetActive(false);
                    asd1 = true;
                    this.gameObject.SetActive(false);
                }
        }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            Takepills.SetActive(false);

        }
    }
}
