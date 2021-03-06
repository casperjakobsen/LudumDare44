﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    [SerializeField] private int audioSourceCount = 10;

    Queue<AudioSource> audioSources = new Queue<AudioSource>();
    void Awake()
    {
        for (int i=0; i<audioSourceCount; i++)
        {
            GameObject obj = new GameObject();
            obj.transform.parent = transform;
            obj.name = "AudioObj";
            obj.AddComponent<AudioSource>();
            audioSources.Enqueue(obj.GetComponent<AudioSource>());
        }
    }

    public void PlaySound(AudioClip clip, float volume, float pitch)
    {
        AudioSource src = audioSources.Dequeue();
        audioSources.Enqueue(src);
        src.volume = volume;
        src.pitch = pitch;
        src.clip = clip;
        src.Play();
    }
}
