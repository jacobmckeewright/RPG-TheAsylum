using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IE_SetInteractableState : InteractableEffect
{
    [SerializeField]
    [Tooltip("State Interactable to change state on")]
    StateInteractable stateInteractable = null;

    [SerializeField]
    [Tooltip("State to set it to")]
    int newState = 0;

    public void Reset()
    {
        if (stateInteractable == null)
            stateInteractable = GetComponent<StateInteractable>();
    }

    public override void Fire()
    {
        if (stateInteractable != null)
        {
            if (!stateInteractable.SetState(newState))
            {
                Debug.LogWarning($"[Interactable] [SetInteractableState] new state {newState} is invalid");
            }
        }
        else
        {
            Debug.LogWarning("[Interactable] [SetInteractableState] targetAnimator is null");
        }
    }

}
