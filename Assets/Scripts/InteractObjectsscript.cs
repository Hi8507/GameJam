using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObjectsscript : MonoBehaviour
{
    public bool tape= false;
    public bool bed=false;
    public bool bookshelf = false;
    public bool computer = false;   //these are for interacting with the object and then character does or says somehting about it

    public AudioSource TapeAU;
    public AudioSource BedAU;
    public AudioSource bookAU;
    public AudioSource computerAU;
    public AudioSource takenAU;

    public GameObject Interact;

    public bool taken= false; //if the tape is taken or not

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
            if(tape== true)
            {
              TapeAU.Play();
                if (taken == false)
                {

                }
            }


        }
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            if (tape == true)
            {
                TapeAU.Play();
                if (taken == false)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {

                    }
                }
            }


        }
    }

}
