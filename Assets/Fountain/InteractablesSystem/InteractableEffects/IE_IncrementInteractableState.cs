using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IE_IncrementInteractableState : InteractableEffect
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
                if (stateInteractable.curState == stateInteractable.MaxStates - 1)
                {
                    stateInteractable.SetState(0);
                }
                else
                    stateInteractable.SetState(stateInteractable.curState + 1);
            }
            else
                Debug.LogWarning("[Interactable] [IncrementState] Can't increment states as none have been setup yet");

        }
        else
        {
            Debug.LogWarning("[Interactable] [IncrementState] stateInteractable is null");
        }
    }
}
