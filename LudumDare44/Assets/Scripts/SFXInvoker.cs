using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXInvoker : MonoBehaviour
{
    SFX sfx;
    [SerializeField] List<AudioClip> clips;
    [SerializeField] float volume = 1;
    [SerializeField] float pitchMin = 1;
    [SerializeField] float pitchMax = 1;
    void Awake()
    {
        sfx = GameObject.FindGameObjectWithTag("SFX").GetComponent<SFX>();
    }

    public void InvokeSound()
    {
        int index = Mathf.FloorToInt(Random.value * clips.Count);
        AudioClip clip = clips[index];

        float pitch = pitchMin + Random.value * (pitchMax - pitchMin); 
        sfx.PlaySound(clip, volume, pitch);
    }
}
