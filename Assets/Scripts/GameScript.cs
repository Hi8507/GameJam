using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class GameScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] public float RemainingTime;

    public  int StartTime=1;
    public int startNumber = 59;
    bool Timerstop=false;

    public int seconds;




    // Start is called before the first frame update
    public void Start()
   {
    

   }
    // Update is called once per frame
    void Update()
    {
       
            if (StartTime >= 0)
            { if (Timerstop == false)
                {
                    // StartTime = 0;
                    StartTime = 0;
                    RemainingTime += Time.deltaTime;
                    StartTime += startNumber;
                    seconds = Mathf.FloorToInt(RemainingTime % 60);

                    StartTime -= seconds;
                    timerText.text = "Time: " + StartTime.ToString();

                }
            }

            else if (StartTime < 0)
            {
                Timerstop = true;
                RemainingTime = 0;
                seconds = 0;
            }
            // StartTime = 1;
           }
     



      
       
     
       
    
}