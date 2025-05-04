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

public class NewGamescript : MonoBehaviour
{
    [Header("UI")]
    public UnityEngine.UI.Slider SanityMeter;
    public TextMeshProUGUI timerText;

    [Header("Gameplay Settings")]
    public float maxSanity = 100f;
    public float sanityDecayRate = 5f; // points per second

    private float currentSanity;
    private float elapsedTime = 0f;
    private bool isGameRunning = true;

    [Header("Pills")]
    public GameObject Yellowpill, Pinkpill, purplepill;

    [Header("Hands")]
    public GameObject[] hands; // assign all in inspector

    void Start()
    {
        currentSanity = maxSanity;
        SanityMeter.maxValue = maxSanity;
        SanityMeter.value = currentSanity;

        Debug.Log("Sanity initialized: " + currentSanity);
    }

    void Update()
    {
        if (!isGameRunning) return;

        elapsedTime += Time.deltaTime;
       // timerText.text = $"Time: {Mathf.FloorToInt(elapsedTime)}s";

        // Sanity decays over time
        currentSanity -= sanityDecayRate * Time.deltaTime;

        // Add pill sanity points once
        currentSanity += ConsumePillSanity(Yellowpill);
        currentSanity += ConsumePillSanity(Pinkpill);
        currentSanity += ConsumePillSanity(purplepill);

        // Add hand sanity points once
        foreach (var hand in hands)
        {
            currentSanity += ConsumeHandSanity(hand);
        }

        // Clamp and update UI
        currentSanity = Mathf.Clamp(currentSanity, 0, maxSanity);
        SanityMeter.value = currentSanity;

        // Debug log
        Debug.Log("Sanity: " + currentSanity);

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
        if (trigger != null && trigger.SanityPoints > 0)
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

