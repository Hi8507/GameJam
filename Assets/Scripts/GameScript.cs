using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;
using UnityEngine.SceneManagement;

public class GameScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] public float RemainingTime;
    public Slider SanityMeter;

    public  int StartTime=1;
    public int startNumber = 59;
    bool Timerstop=false;

    public int seconds;

    public GameObject Yellowpill, Pinkpill;





    // Start is called before the first frame update
    public void Start()
   {
    

   }
    // Update is called once per frame
    void Update()
    {
       
            if (StartTime >= 0){ 

                if (Timerstop == false){

                    // StartTime = 0;
                    StartTime = 0;
                    RemainingTime += Time.deltaTime;
                    StartTime += startNumber+ Yellowpill.GetComponent<TriggerPills>().SanityPoints;
                    seconds = Mathf.FloorToInt(RemainingTime % 60);

                    StartTime -= seconds;
                 //   timerText.text = "Time: " + StartTime.ToString();
                 SanityMeter.value = StartTime;

                }
            }

            else if (StartTime < 0)
            {
                Timerstop = true;
                RemainingTime = 0;
            seconds = 0;

            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(SceneManager.GetActiveScene);

        }
            // StartTime = 1;
           }
     



      
       
     
       
    
}