using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IE_TriggerUnityEvent : InteractableEffect
{
    [SerializeField]
    private UnityEvent unityEvent = null;

    public override void Fire()
    {
        if (unityEvent != null)
            unityEvent.Invoke();

    }

}
