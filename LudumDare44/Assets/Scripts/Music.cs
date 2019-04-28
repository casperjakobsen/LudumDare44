using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] float fadeInTime = 1f;
    [SerializeField] float maxVolume = 1f;
    [SerializeField] AnimationCurve fadeInCurve;

    AudioSource audio;
    void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Time.time < fadeInTime)
        {
            audio.volume = fadeInCurve.Evaluate(Time.time/fadeInTime) * maxVolume;
        }
        else
        {
            audio.volume = maxVolume;
        }
    }
}
