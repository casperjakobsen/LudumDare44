using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] float fadeInTime = 1f;
    [SerializeField] float maxVolume = 1f;
    [SerializeField] AnimationCurve fadeInCurve;

    public static bool instantiated;
    AudioSource audio;
    void Awake()
    {
        if (instantiated)
        {
            Destroy(gameObject);
        }
        audio = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
        instantiated = true;
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
