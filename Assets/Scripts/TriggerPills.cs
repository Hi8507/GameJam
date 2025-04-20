using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TriggerPills : MonoBehaviour
{

    public GameObject Takepills;

    public int SanityPoints;
    // Start is called before the first frame update
    void Start()
    {
        Takepills.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character"))
        {
        Takepills.SetActive(true);
        
        }
        }
    private void OnTriggerStay(Collider other)
    {
        SanityPoints = 0;
        if (other.CompareTag("Character"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                SanityPoints = 20;
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
