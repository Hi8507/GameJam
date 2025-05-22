using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public float delayInSeconds = 2f;

    void Start()
    {
        audioSource.PlayDelayed(delayInSeconds);
    }
}
