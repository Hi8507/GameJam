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
using UnityEngine.UIElements;

public class GameScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] public float RemainingTime;
    public UnityEngine.UI.Slider SanityMeter;

    public  int StartTime=0;
    public int startNumber = 100;
    bool Timerstop=false;

    public int seconds;
    public int points1;

    public GameObject Yellowpill, Pinkpill,purplepill;


    public GameObject hand1, hand2, hand3, hand4, hand5, hand6, hand11, hand12, hand13, hand14, hand15, hand16, hand17, hand18, hand19, hand110, hand111;


    // Start is called before the first frame update
    public void Start()
   {
    

   }
    // Update is called once per frame
    void Update()
    {

      //  points1 = hand1.GetComponent<handtrigger>().SanityPoints + hand2.GetComponent<handtrigger>().SanityPoints + hand3.GetComponent<handtrigger>().SanityPoints + hand4.GetComponent<handtrigger>().SanityPoints + hand5.GetComponent<handtrigger>().SanityPoints + hand6.GetComponent<handtrigger>().SanityPoints + hand11.GetComponent<handtrigger>().SanityPoints + hand12.GetComponent<handtrigger>().SanityPoints + hand13.GetComponent<handtrigger>().SanityPoints + hand14.GetComponent<handtrigger>().SanityPoints + hand15.GetComponent<handtrigger>().SanityPoints + hand16.GetComponent<handtrigger>().SanityPoints + hand17.GetComponent<handtrigger>().SanityPoints + hand18.GetComponent<handtrigger>().SanityPoints + hand19.GetComponent<handtrigger>().SanityPoints + hand110.GetComponent<handtrigger>().SanityPoints + hand111.GetComponent<handtrigger>().SanityPoints;

        // StartTime = 1;
    }
    private void FixedUpdate()
    {
        if (StartTime >0)
        {

            if (Timerstop == false)
            {

                // StartTime = 0;
               StartTime = 0;
                RemainingTime += Time.deltaTime;
                StartTime += startNumber + Yellowpill.GetComponent<TriggerPills>().SanityPoints + Pinkpill.GetComponent<TriggerPills>().SanityPoints + purplepill.GetComponent<TriggerPills>().SanityPoints + points1;
                seconds = Mathf.FloorToInt(RemainingTime % 60);

                StartTime -= seconds;
                //   timerText.text = "Time: " + StartTime.ToString();
                SanityMeter.value = StartTime;

            }
        }

        else if (StartTime <= 0)
        {
            Timerstop = true;
            RemainingTime = 0;
            seconds = 0;

            ReloadScene();

        }
    }

    private void ReloadScene()
    {
        // Get the currently active scene and reload it
        UnityEngine.SceneManagement.Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }








}