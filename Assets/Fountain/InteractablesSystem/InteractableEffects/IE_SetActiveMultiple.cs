using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IE_SetActiveMultiple : InteractableEffect
{
    [SerializeField]
    [Tooltip("Target Gameobjects to setActive")]
    GameObject[] targetObjects = null;

    [SerializeField]
    [Tooltip("State to which we are changing")]
    bool state = false;

    public override void Fire()
    {
        if (targetObjects != null)
        {
            for (int i = 0; i < targetObjects.Length; i++)
            {
                if (targetObjects[i] != null)
                    targetObjects[i].SetActive(state);
            }
        }
        else
            Debug.LogWarning("[Interactable] [SetActiveMultiple] targetObjects array is null");
    }
}