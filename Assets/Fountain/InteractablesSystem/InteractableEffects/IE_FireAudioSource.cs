using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IE_FireAudioSource : InteractableEffect
{
    [SerializeField]
    [Tooltip("Plays audiosource on being fired")]
    private AudioSource audioSource;

    public void Reset()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public override void Fire()
    {
        if (audioSource != null)
            audioSource.Play();
        else
            Debug.LogWarning("[Interactable] [FireAudioSource] audioSource is null");
    }
}
