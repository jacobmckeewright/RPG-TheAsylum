using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class LadderScript : MonoBehaviour
{
    public Transform chController;
    bool inside = false;
    public float speedUpDown = 3.2f;
    public FirstPersonController FPSInput;

    private void Start()
    {
        FPSInput = GetComponent<FirstPersonController>();
        inside = false;
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag =="Ladder")
        {
            FPSInput.enabled = false;
            inside = !inside;
        }
    }
    private void Update()
    {
        if(inside == true && Input.GetKey("w"))
        {
            chController.transform.position += Vector3.up /
                speedUpDown;
        }
        if (inside == true && Input.GetKey("s"))
        {
            chController.transform.position += Vector3.down /
                speedUpDown;
        }
    }
}
