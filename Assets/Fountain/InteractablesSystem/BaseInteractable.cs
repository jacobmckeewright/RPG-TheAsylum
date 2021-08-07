using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInteractable : MonoBehaviour
{
    public abstract void Fire();

    public abstract void OnCameraInputTriggered(float hitDistance);
}
