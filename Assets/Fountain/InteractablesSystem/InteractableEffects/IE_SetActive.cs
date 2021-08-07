using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IE_SetActive : InteractableEffect
{
    [SerializeField]
    [Tooltip("Target Gameobject to setActive")]
    GameObject targetObject = null;

    [SerializeField]
    [Tooltip("State to which we are changing")]
    bool state = false;

    public override void Fire()
    {
        if (targetObject != null)
            targetObject.SetActive(state);
        else
            Debug.LogWarning("[Interactable] [SetObjectActive] targetObject is null");
    }
}