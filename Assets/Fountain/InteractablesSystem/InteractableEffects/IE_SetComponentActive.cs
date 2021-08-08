using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IE_SetComponentActive : InteractableEffect
{

    [SerializeField]
    [Tooltip("MonoBehaviour to set the enabled state to")]
    private MonoBehaviour targetComponent;

    [SerializeField]
    [Tooltip("State to which we are changing")]
    bool state = false;

    public override void Fire()
    {
        if (targetComponent != null)
            targetComponent.enabled = state;
        else
            Debug.LogWarning("[Interactable] [SetComponentActive] targetComponent is null");
    }
}
