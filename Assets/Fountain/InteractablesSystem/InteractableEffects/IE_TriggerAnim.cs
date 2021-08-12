using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IE_TriggerAnim : InteractableEffect
{
    [SerializeField]
    [Tooltip("Animator to set trigger on")]
    private Animator targetAnimator = null;

    [SerializeField]
    [Tooltip("Name of the Animator property to which we are firing")]
    private string triggerName = "";

    [SerializeField]
    [Tooltip("Delay in seconds to fire the animation trigger")]
    private float delay = 0.0f;

    public void Reset()
    {
        if (targetAnimator == null)
            targetAnimator = GetComponent<Animator>();
    }

    public override void Fire()
    {
        if (targetAnimator != null)
        {
            if (delay > 0)
            {
                StartCoroutine(DelayTrigger());
            }
            else
                targetAnimator.SetTrigger(triggerName);
        }
        else
            Debug.LogWarning("[Interactable] [TriggerAnim] targetAnimator is null");
    }

    private IEnumerator DelayTrigger()
    {
        yield return new WaitForSeconds(delay);
        targetAnimator.SetTrigger(triggerName);
    }
}
