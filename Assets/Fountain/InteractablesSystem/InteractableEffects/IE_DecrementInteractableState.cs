using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IE_DecrementInteractableState : InteractableEffect
{
    [SerializeField]
    [Tooltip("State Interactable to change state on")]
    StateInteractable stateInteractable;

    public void Reset()
    {
        if (stateInteractable == null)
            stateInteractable = GetComponent<StateInteractable>();
    }

    public override void Fire()
    {
        if (stateInteractable != null)
        {
            if (stateInteractable.MaxStates > 0)
            {
                if (stateInteractable.curState == 0)
                {
                    stateInteractable.SetState(stateInteractable.MaxStates - 1);
                }
                else
                    stateInteractable.SetState(stateInteractable.curState - 1);
            }
            else
                Debug.LogWarning("[Interactable] [DecrementState] Can't decrement states as none have been setup yet");

        }
        else
        {
            Debug.LogWarning("[Interactable] [DecrementState] stateInteractable is null");
        }
    }
}
