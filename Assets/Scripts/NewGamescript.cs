using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class NewGamescript : MonoBehaviour
{
    [Header("UI")]
    public UnityEngine.UI.Slider SanityMeter;
    public TextMeshProUGUI timerText;

    [Header("Gameplay Settings")]
    public float maxSanity = 100f;
    public float StartSanity = 100f;
    public float sanityDecayRate = 5f; // points per second
    public float intensifiedDecayMultiplier = 2f;

    public float currentSanity;
    private float elapsedTime = 0f;
    private bool isGameRunning = true;
    public GameObject Nurse1;
    public GameObject Nurse2;



    [Header("Pills")]
    public GameObject[] pills;

    [Header("Hands")]
    public GameObject[] hands; // assign all in inspector

    public void Start()
    {
        currentSanity = StartSanity;
        SanityMeter.maxValue = maxSanity;
        SanityMeter.value = currentSanity;

        Debug.Log("Sanity initialized: " + currentSanity);
    }

    public void Update()
    {
        if (!isGameRunning) return;

        elapsedTime += Time.deltaTime;
        //timerText.text = $"Time: {Mathf.FloorToInt(elapsedTime)}s";

        // Sanity decays over time
        float appliedDecayRate = sanityDecayRate; // Start with normal rate

        bool nurse1Active = Nurse1.GetComponent<EnterHeartrate>().SanityOpen;

        bool nurse2Active = Nurse2.GetComponent<EnterHeartrate>().SanityOpen;

        if (nurse1Active || nurse2Active)
        {
            appliedDecayRate *= intensifiedDecayMultiplier;
            Debug.Log("Secondary depletion active");
        }
        else
        {
            Debug.Log("Normal decay rate applied");
        }

        currentSanity -= appliedDecayRate * Time.deltaTime;

        Debug.Log("Nurse1: " + Nurse1.GetComponent<EnterHeartrate>().SanityOpen);

        Debug.Log("Nurse2: " + Nurse2.GetComponent<EnterHeartrate>().SanityOpen);

        // Add pill sanity points once
        foreach (var pill in pills)
        {
            currentSanity += ConsumePillSanity(pill);
        }
        //currentSanity += ConsumePillSanity(Yellowpill);
        //currentSanity += ConsumePillSanity(Pinkpill);
        //currentSanity += ConsumePillSanity(purplepill);
        //currentSanity += ConsumePillSanity(newpill);


        // Add hand sanity points once
        foreach (var hand in hands)
        {
            currentSanity += ConsumeHandSanity(hand);
        }

        // Clamp and update UI
        currentSanity = Mathf.Clamp(currentSanity, 0, maxSanity);
        SanityMeter.value = currentSanity;

        // Debug log
       // Debug.Log("Sanity: " + currentSanity);

        if (currentSanity <= 0f)
        {
            isGameRunning = false;
            Debug.Log("Sanity depleted. Reloading...");
            ReloadScene();
        }
    }



    float ConsumePillSanity(GameObject pill)
    {
        var trigger = pill.GetComponent<TriggerPills>();
        if (trigger != null && trigger.SanityPoints > 0)
        {
            float points = trigger.SanityPoints;
            trigger.SanityPoints = 0;
            return points;
        }
        return 0;
    }

    float ConsumeHandSanity(GameObject hand)
    {
        var trigger = hand.GetComponent<handtrigger>();
        if (trigger != null && trigger.SanityPoints < 0)
        {
            float points = trigger.SanityPoints;
            trigger.SanityPoints = 0;
            return points;
        }
        return 0;
    }

    private void ReloadScene()
    {


        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

