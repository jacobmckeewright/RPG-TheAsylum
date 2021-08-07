using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CondtionalInteractable : BaseInteractable
{
    [Space(5)]
    [SerializeField]
    [Tooltip("Sets this interactable ignore all atttempts at firing after the first time")]
    private bool onlyFireOnce = false;

    [SerializeField]
    [Tooltip("Seconds until this interactable can be fired again. (<=0 to fire every time)")]
    private float delayToFireAgain = 0.0f;
    
    [Header("Camera Trigger")]
    [SerializeField]
    [Tooltip("Allows to be fired from an (InteractableCameraInputTrigger) object")]
    private bool isTriggeredFromCameraInput = false;

    [SerializeField]
    [Tooltip("Min distance to be fired from (InteractableCameraInputTrigger) object. (Can be <=0 to ignore)")]
    private float minDistanceToFireFromCameraInput = 0.0f;

    [Header("Trigger Volume")]
    [SerializeField]
    [Tooltip("Sets to be fired on the OnTriggerEnter Callback (Requires Collider w/ IsTriggerOn attached to same object)")]
    private bool isFiredOnTriggerEnter = false;

    [SerializeField]
    [Tooltip("Sets to fired on the OnTriggerExit Callback (Requires Collider w/ IsTriggerOn attached to same object)")]
    private bool isFiredOnTriggerExit = false;
    
    [SerializeField]
    [Tooltip("Set to only fire when triggered by tagged object. (Can be empty to allow all)")]
    private string triggerTag = "";
    

    //Hidden stuff
    [HideInInspector]
    public InteractableEffect[] interactableEffects;

    [HideInInspector]
    public ConditionalStatement[] conditionalStatements;

    [HideInInspector]
    public bool hasFired = false;

    [HideInInspector]
    public DateTime lastTimeFired = new DateTime(0);


    public void Start()
    {
        if (isFiredOnTriggerEnter || isFiredOnTriggerExit)
        {
            if (GetComponent<Collider>() == null)
            {
                Debug.LogWarning($"[Interactable] {gameObject.name} does not have a collider attached but has been set to fire on trigger");
            }
        }
    }

    public override void OnCameraInputTriggered(float hitDistance)
    {
        if (!CheckCanFire())
            return;

        if (isTriggeredFromCameraInput)
        {
            if (minDistanceToFireFromCameraInput > 0 && minDistanceToFireFromCameraInput >= hitDistance)
                return;

            Fire();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!CheckCanFire())
            return;

        if (isFiredOnTriggerEnter)
        {
            if (string.IsNullOrEmpty(triggerTag) || other.CompareTag(triggerTag))
            {
                Fire();
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (!CheckCanFire())
            return;

        if (isFiredOnTriggerExit)
        {
            if (string.IsNullOrEmpty(triggerTag) || other.CompareTag(triggerTag))
            {
                Fire();
            }
        }
    }

    private bool CheckCanFire()
    {
        if (hasFired && onlyFireOnce)
            return false;

        if (delayToFireAgain >= 0 && (DateTime.Now - lastTimeFired).TotalSeconds < delayToFireAgain)
            return false;


        if (!CheckConditionalStatements())
            return false;

        return true;
    }

    public bool CheckConditionalStatements()
    {
        //conditional checks 
        for (int i = 0; i < conditionalStatements.Length; i++)
        {
            if (conditionalStatements[i].component != null)
            {
                Component component = conditionalStatements[i].component;
                string variableName = conditionalStatements[i].variableName;
                string variableValue = conditionalStatements[i].value;
                LogicOperator logicOperator = conditionalStatements[i].condition;

                //Attempts to get the variable on the compponent specified
                System.Reflection.PropertyInfo propertyInfo = component.GetType().GetProperty(variableName);

                if (propertyInfo != null)
                {
                    object value = propertyInfo.GetValue(component, null);
                    Type valueType = value.GetType();

                    switch (Type.GetTypeCode(valueType))
                    {
                        case TypeCode.Decimal:
                        case TypeCode.Int16:
                        case TypeCode.Int32:
                        case TypeCode.Int64:
                        case TypeCode.UInt16:
                        case TypeCode.UInt32:
                        case TypeCode.UInt64:
                            int intValue = 0;
                            if (int.TryParse(conditionalStatements[i].value, out intValue))
                            {
                                switch (logicOperator)
                                {
                                    //check the opposing value to return false on any fails
                                    case LogicOperator.EqualTo:
                                        if (intValue != (int)value)
                                            return false;
                                        break;
                                    case LogicOperator.NotEqualTo:
                                        if (intValue == (int)value)
                                            return false;
                                        break;
                                    case LogicOperator.GreaterThan:
                                        if (intValue <= (int)value)
                                            return false;
                                        break;
                                    case LogicOperator.GreaterThanOrEqual:
                                        if (intValue < (int)value)
                                            return false;
                                        break;
                                    case LogicOperator.LessThan:
                                        if (intValue >= (int)value)
                                            return false;
                                        break;
                                    case LogicOperator.LessThanOrEqual:
                                        if (intValue > (int)value)
                                            return false;
                                        break;
                                };
                            }
                            else
                            {
                                Debug.LogError($"[CondtionalInteractable] Could not parse value [{variableValue}] to type integer on Variable Name [{variableName}] for Component [{component.name}]");
                                return false;
                            }
                            break;

                        case TypeCode.Double:
                        case TypeCode.Single:
                            float floatValue = 0;
                            if (float.TryParse(conditionalStatements[i].value, out floatValue))
                            {
                                switch (logicOperator)
                                {
                                    //check the opposing value to return false on any fails
                                    case LogicOperator.EqualTo:
                                        if (floatValue != (float)value)
                                            return false;
                                        break;
                                    case LogicOperator.NotEqualTo:
                                        if (floatValue == (float)value)
                                            return false;
                                        break;
                                    case LogicOperator.GreaterThan:
                                        if (floatValue <= (float)value)
                                            return false;
                                        break;
                                    case LogicOperator.GreaterThanOrEqual:
                                        if (floatValue < (float)value)
                                            return false;
                                        break;
                                    case LogicOperator.LessThan:
                                        if (floatValue >= (float)value)
                                            return false;
                                        break;
                                    case LogicOperator.LessThanOrEqual:
                                        if (floatValue > (float)value)
                                            return false;
                                        break;
                                };
                            }
                            else
                            {
                                Debug.LogError($"[CondtionalInteractable] Could not find parse value [{variableValue}] to type float on Variable Name [{variableName}] for Component [{component.name}]");
                                return false;
                            }
                            break;

                        case TypeCode.String:
                            string stringValue = conditionalStatements[i].value;
                            switch (logicOperator)
                            {
                                case LogicOperator.EqualTo:
                                    if (stringValue != (string)value)
                                        return false;
                                    break;
                                case LogicOperator.NotEqualTo:
                                    if (stringValue == (string)value)
                                        return false;
                                    break;
                                default: //all else
                                    Debug.LogError($"[CondtionalInteractable] Invalid logic operator [{logicOperator}] for [{variableValue}] to type string on Variable Name [{variableName}] for Component [{component.name}]");
                                    return false;
                                    //break;
                            }
                            break;

                        case TypeCode.Boolean:
                            bool boolValue = false;
                            if (bool.TryParse(conditionalStatements[i].value, out boolValue))
                            {
                                switch (logicOperator)
                                {
                                    case LogicOperator.EqualTo:
                                        if (boolValue != (bool)value)
                                            return false;
                                        break;
                                    case LogicOperator.NotEqualTo:
                                        if (boolValue == (bool)value)
                                            return false;
                                        break;
                                    default: //all else
                                        Debug.LogError($"[CondtionalInteractable] Invalid logic operator [{logicOperator}] for [{variableValue}] to type bool on Variable Name [{variableName}] for Component [{component.name}]");
                                        return false;
                                        //break;
                                }
                            }
                            else
                            {
                                Debug.LogError($"[CondtionalInteractable] Could not find parse value [{variableValue}] to type boolean on Variable Name [{variableName}] for Component [{component.name}]");
                                return false;
                            }
                            break;
                        default:
                            Debug.LogError($"[CondtionalInteractable] Unhandled type [{Type.GetTypeCode(valueType)}] on [{variableName}] for component [{component.name}]");
                            return false;
                    }
                }
                else
                {
                    Debug.LogError($"[CondtionalInteractable] Could not find variable [{variableName}] for component [{component.name}]");
                    return false;
                }
            }
            else
            {
                Debug.LogError($"[CondtionalInteractable] Component referenced in condition is null or empty");
                return false;
            }
        }

        return true;
    }


    public override void Fire()
    {
        foreach (var effect in interactableEffects)
        {
            effect.Fire();
        }
        

        hasFired = true;
        lastTimeFired = DateTime.Now;
    }

    [Serializable]
    public struct ConditionalStatement
    {
        public Component component;
        public string variableName;
        public LogicOperator condition;
        public string value;
    }

    public enum LogicOperator
    {
        EqualTo,
        NotEqualTo,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual
    }

}
