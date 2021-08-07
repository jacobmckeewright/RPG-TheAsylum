using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
public class DoorRaycast : MonoBehaviour
{
    [SerializeField] private int rayLength = 5;
    [SerializeField] private LayerMask layerMaskInteract;
    [SerializeField] private string excludeLayerName = null;

    private MyDoorController raycastObj;

    [SerializeField] private KeyCode openDoorKey = KeyCode.E;

    [SerializeField] private Image crosshair = null;
    private bool isCrosshairActive;
    private bool doOnce;

    private const string interactableTag = "InteractiveObject";

    private void Update()
    {
        RaycastHit;
        Vector3.fwd = transform.TransformDirection(Vector3.forward);

        int mask = 1 << LayerMask.NameToLayer(excludeLayerName) | layerMaskInteract.value;

        if (Physics.Raycast(transform.position, fwd, out hit, rayLength, mask))
        {
            if (!doOnce)
            {
                raycastObj = hit.collider.gameObject.GetCompoment<MyDoorController>();
                CrosshairChange(true);

            }
            isCrosshairActive = true;
            doOnce = true;
            if (Input.GetKeyDown(openDoorKey))
            {
                raycastObj.PlayAnimation();
            }
        }
    }

    else
    {
    if(isCrosshairActive)
        {
        if
}

}
*/