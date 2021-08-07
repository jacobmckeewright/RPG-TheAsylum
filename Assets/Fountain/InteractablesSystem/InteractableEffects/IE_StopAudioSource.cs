using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IE_StopAudioSource : InteractableEffect
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
        audioSource.Stop();
    }

}
