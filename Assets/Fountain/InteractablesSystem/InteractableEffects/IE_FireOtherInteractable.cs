using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IE_FireOtherInteractable : InteractableEffect
{
    [SerializeField]
    [Tooltip("Interactable to force to fire (Will happen synchronously)")]
    BaseInteractable interactable = null;

    public override void Fire()
    {
        if (interactable != null)
        {
            if (interactable.CheckCanFire())
                interactable.Fire();
        }
        else
            Debug.LogWarning("[Interactable] [FireOtherInteractable] interactable is null");
    }
}
