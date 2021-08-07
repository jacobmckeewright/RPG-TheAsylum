using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateInteractable : BaseInteractable
{
    [SerializeField]
    [Tooltip("Seconds until this interactable can be fired again. (<=0 to fire every time)")]
    private float delayToFireAgain = 0.0f;

    [Header("Camera Trigger")]
    [SerializeField]
    [Tooltip("Allows to be fired from an (InteractableCameraInputTrigger) object")]
    private bool isTriggeredFromCameraInput = false;

    [SerializeField]
    [Tooltip("Min distance to be fired from (InteractableCameraInputTrigger) object. (Can be <=0 to ignore)")]
    private float minDistanceToFireFromCameraInput = 0.0f;

    [Header("Trigger Volume")]
    [SerializeField]
    [Tooltip("Sets to be fired on the OnTriggerEnter Callback (Requires Collider w/ IsTriggerOn attached to same object)")]
    private bool isFiredOnTriggerEnter = false;

    [SerializeField]
    [Tooltip("Sets to fired on the OnTriggerExit Callback (Requires Collider w/ IsTriggerOn attached to same object)")]
    private bool isFiredOnTriggerExit = false;

    [SerializeField]
    [Tooltip("Set to only fire when triggered by tagged object. (Can be empty to allow all)")]
    private string triggerTag = "";

    [Space(10)]
    [SerializeField]
    private int startingState = 0;

    //Hidden stuff
    [HideInInspector]
    public StateInteractable_EffectList[] interactableEffects;

    [HideInInspector]
    public DateTime lastTimeFired = new DateTime(0);

    public int curState { private set; get; }

    public void Start()
    {
        if (isFiredOnTriggerEnter || isFiredOnTriggerExit)
        {
            if (GetComponent<Collider>() == null)
            {
                Debug.LogWarning($"[StateInteractable] {gameObject.name} does not have a collider attached but has been set to fire on trigger");
            }
        }

        curState = startingState;
    }

    public override void OnCameraInputTriggered(float hitDistance)
    {
        if (!CheckCanFire())
            return;

        if (isTriggeredFromCameraInput)
        {
            if (minDistanceToFireFromCameraInput > 0 && minDistanceToFireFromCameraInput >= hitDistance)
                return;

            Fire();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!CheckCanFire())
            return;

        if (isFiredOnTriggerEnter)
        {
            if (string.IsNullOrEmpty(triggerTag) || other.CompareTag(triggerTag))
            {
                Fire();
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (!CheckCanFire())
            return;

        if (isFiredOnTriggerExit)
        {
            if (string.IsNullOrEmpty(triggerTag) || other.CompareTag(triggerTag))
            {
                Fire();
            }
        }
    }

    private bool CheckCanFire()
    {
        if (delayToFireAgain >= 0 && (DateTime.Now - lastTimeFired).TotalSeconds < delayToFireAgain)
            return false;

        return true;
    }

    public override void Fire()
    {
        StateInteractable_EffectList effectList = interactableEffects[curState];
        foreach (var effect in effectList.stateInteractableEffects)
        {
            effect.Fire();
        }

        lastTimeFired = DateTime.Now;
    }

    public bool SetState(int newState)
    {
        if (newState >= 0 || newState < interactableEffects.Length)
        {
            curState = newState;
            return true;
        }

        return false;
    }
}

[Serializable]
public struct StateInteractable_EffectList
{
    [SerializeField]
    public InteractableEffect[] stateInteractableEffects;
}



