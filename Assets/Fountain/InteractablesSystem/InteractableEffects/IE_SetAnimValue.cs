using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(IE_SetAnimValue))]
public class IE_SetAnimValue_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        IE_SetAnimValue.AnimValueType curType = (IE_SetAnimValue.AnimValueType)serializedObject.FindProperty("valueType").enumValueIndex;

        if (curType == IE_SetAnimValue.AnimValueType.Bool)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("boolValue"));
        }
        else if (curType == IE_SetAnimValue.AnimValueType.Integer)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("intValue"));
        }
        else if (curType == IE_SetAnimValue.AnimValueType.Float)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("floatValue"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}

#endif

public class IE_SetAnimValue : InteractableEffect
{
    [SerializeField]
    [Tooltip("Animator to set values to")]
    private Animator targetAnimator = null;

    [SerializeField]
    [Tooltip("Name of the animator property to change")]
    private string valueName = "";

    [SerializeField]
    [Tooltip("Data Type of the animator property to change")]
    private AnimValueType valueType = AnimValueType.Bool;

    [SerializeField]
    [Tooltip("Time in seconds before value is set")]
    private float delay = 0.0f;

    [HideInInspector]
    public bool boolValue = false;
    
    [HideInInspector]
    public int intValue = 0;

    [HideInInspector]
    public float floatValue = 0.0f;

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
                StartCoroutine(DelayFire());
            else
            {
                switch (valueType)
                {
                    case AnimValueType.Bool:
                        targetAnimator.SetBool(valueName, boolValue);
                        break;
                    case AnimValueType.Integer:
                        targetAnimator.SetInteger(valueName, intValue);
                        break;
                    case AnimValueType.Float:
                        targetAnimator.SetFloat(valueName, floatValue);
                        break;
                }
            }
        }
        else
            Debug.LogWarning("[Interactable] [TriggerAnim] targetAnimator is null");
    }

    public IEnumerator DelayFire()
    {
        yield return new WaitForSeconds(delay);

        switch (valueType)
        {
            case AnimValueType.Bool:
                targetAnimator.SetBool(valueName, boolValue);
                break;
            case AnimValueType.Integer:
                targetAnimator.SetInteger(valueName, intValue);
                break;
            case AnimValueType.Float:
                targetAnimator.SetFloat(valueName, floatValue);
                break;
        }
    }

    public enum AnimValueType
    {
        Bool,
        Integer,
        Float,
    };
}



