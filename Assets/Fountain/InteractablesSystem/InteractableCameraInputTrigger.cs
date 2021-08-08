using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCameraInputTrigger : MonoBehaviour
{
    [SerializeField] 
    [Tooltip("Camera we are using fire raycasts from")]
    private Camera triggerCamera = null;
                     
    [SerializeField] 
    [Tooltip("Keys we are listening to for OnKeyDown")]
    private KeyCode[] keysOnDownToFire = { KeyCode.E };
                     
    [SerializeField] 
    [Tooltip("Set if we are listening to OnMouseButtonDown(0)")]
    private bool firesOnLeftMouseInput = true;
    [SerializeField]
    [Tooltip("Set if we are listening to OnMouseButtonDown(1)")]
    private bool firesOnRightMouseInput = false;
    [SerializeField]
    [Tooltip("Set if we are listening to OnMouseButtonDown(2)")]
    private bool firesOnMiddleMouseInput = false;

    [Tooltip("Distance we are firing raycasts to check input")]
    [SerializeField] private float distToRayCast = 100.0f;

    private bool hasRayCast = false;
    private Vector3 lastHitPosition = Vector3.zero;

    void Update()
    {
        bool validFire = CheckValidFire();

        if (validFire)
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(triggerCamera.transform.position, triggerCamera.transform.forward, out hitInfo, distToRayCast))
            {
                hasRayCast = true;
                lastHitPosition = hitInfo.point;

                BaseInteractable[] hitInteractables = hitInfo.collider.gameObject.GetComponents<BaseInteractable>();

                if (hitInteractables != null)
                {
                    for (int i = 0; i < hitInteractables.Length; i++)
                    {
                        if (hitInteractables[i] != null)
                        {
                            if (hitInteractables[i].isActiveAndEnabled)
                                hitInteractables[i].OnCameraInputTriggered(hitInfo.distance);
                        }
                    }
                }
            }
        }
    }

    bool CheckValidFire()
    {
        if (keysOnDownToFire != null)
        {
            for (int i = 0; i < keysOnDownToFire.Length; i++)
            {
                if (Input.GetKeyDown(keysOnDownToFire[i]))
                {
                    return true;
                }
            }
        }

        if (Input.GetMouseButtonDown(0) && firesOnLeftMouseInput)
            return true;

        if (Input.GetMouseButtonDown(1) && firesOnRightMouseInput)
            return true;

        if (Input.GetMouseButtonDown(2) && firesOnMiddleMouseInput)
            return true;

        return false;
    }

    private void OnDrawGizmos()
    {
        if (hasRayCast)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(lastHitPosition, 0.25f);
        }
    }

}
