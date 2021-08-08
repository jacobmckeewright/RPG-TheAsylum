using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(CondtionalInteractable))]
public class CondtionalInteractableEditor : Editor
{
    SerializedProperty interactableEffects;
    SerializedProperty conditionals;

    ReorderableList effectList;
    ReorderableList conditionalList;

    private void OnEnable()
    {
        interactableEffects = serializedObject.FindProperty("interactableEffects");
        conditionals = serializedObject.FindProperty("conditionalStatements");

        effectList = new ReorderableList(serializedObject, interactableEffects, true, true, true, true);
        conditionalList = new ReorderableList(serializedObject, conditionals, true, true, true, true);

        effectList.drawElementCallback = DrawListItems;
        effectList.drawHeaderCallback = DrawBaseHeader;
        //effectList.elementHeight = EditorGUIUtility.singleLineHeight;

        conditionalList.drawElementCallback = DrawConditionalListItems;
        conditionalList.drawHeaderCallback = DrawConditionalHeader;
        conditionalList.elementHeight = EditorGUIUtility.singleLineHeight * 5.7f;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        GUILayout.Space(10);

        if (GUILayout.Button("Verify Conditions"))
        {            
            bool result = ((CondtionalInteractable)target).CheckConditionalStatements();
            Debug.Log($"[CondtionalInteractable] Conditions currently equate to {result}");            
        }

        conditionalList.DoLayoutList();

        effectList.DoLayoutList();

        GUILayout.Space(10);

        if (Application.isPlaying)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label($"DEBUG");
            GUILayout.EndHorizontal();

            GUILayout.Label($"Has Fired: {((CondtionalInteractable)target).hasFired}");

            if (GUILayout.Button("Test Fire"))
            {
                ((CondtionalInteractable)target).Fire();
            }

            if (GUILayout.Button("Reset has fired"))
            {
                ((CondtionalInteractable)target).hasFired = false;
            }

        }

        serializedObject.ApplyModifiedProperties();
    }

    //List items
    void DrawListItems(Rect rect, int index, bool isActive, bool isFocused)
    {
        SerializedProperty element = effectList.serializedProperty.GetArrayElementAtIndex(index); // The element in the list

        EditorGUI.LabelField(
            new Rect(rect.x, rect.y, 250, EditorGUIUtility.singleLineHeight),
            element.displayName, "tool tip"
            );

        EditorGUI.PropertyField(
               new Rect(rect.x + 100, rect.y, 300, EditorGUIUtility.singleLineHeight),
               element,
               GUIContent.none
           );
    }

    void DrawConditionalListItems(Rect rect, int index, bool isActive, bool isFocused)
    {
        SerializedProperty element = conditionalList.serializedProperty.GetArrayElementAtIndex(index); // The element in the list
        SerializedProperty componentProperty = element.FindPropertyRelative("component");
        SerializedProperty variableNameProperty = element.FindPropertyRelative("variableName");
        SerializedProperty conditionProperty = element.FindPropertyRelative("condition");
        SerializedProperty valueProperty = element.FindPropertyRelative("value");

        //Title
        EditorGUI.LabelField(
                new Rect(rect.x, rect.y, 250, EditorGUIUtility.singleLineHeight),
                element.displayName
                );

        //Component
        EditorGUI.LabelField(
                new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 1.1f, 250, EditorGUIUtility.singleLineHeight ),
                componentProperty.displayName
                );

        EditorGUI.PropertyField(
               new Rect(rect.x + 100, rect.y + EditorGUIUtility.singleLineHeight * 1.1f, 300, EditorGUIUtility.singleLineHeight),
               componentProperty,
               GUIContent.none);

        //Variable Name
        EditorGUI.LabelField(
                new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 2.2f, 250, EditorGUIUtility.singleLineHeight ),
                variableNameProperty.displayName);

        EditorGUI.PropertyField(
                new Rect(rect.x + 100, rect.y + EditorGUIUtility.singleLineHeight * 2.2f, 300, EditorGUIUtility.singleLineHeight),
                variableNameProperty,
                GUIContent.none);

        EditorGUI.LabelField(
                new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 3.3f, 250, EditorGUIUtility.singleLineHeight),
                conditionProperty.displayName);

        EditorGUI.PropertyField(
                new Rect(rect.x + 100, rect.y + EditorGUIUtility.singleLineHeight * 3.3f, 300, EditorGUIUtility.singleLineHeight),
                conditionProperty,
                GUIContent.none);

        EditorGUI.LabelField(
                new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 4.4f, 250, EditorGUIUtility.singleLineHeight),
                valueProperty.displayName);

        EditorGUI.PropertyField(
                new Rect(rect.x + 100, rect.y + EditorGUIUtility.singleLineHeight * 4.4f, 300, EditorGUIUtility.singleLineHeight),
                valueProperty,
                GUIContent.none);

        //Dissapointing but EditorGUILayout.Popup cannot be contained in reorderable List elements
        /*Component componentVal = (Component)componentProperty.objectReferenceValue;
        if (componentVal != null)
        {
            Type componentType = componentVal.GetType();
            System.Reflection.MemberInfo[] memberNames = componentType.GetMembers();
            string[] options = new string[memberNames.Length];
            for (int i = 0; i < options.Length; i++)
            {
                options[i] = memberNames[i].Name;
            }
            int valChoosen = 0;            
            EditorGUILayout.Popup("Variable to check", valChoosen, options);
        
            //foreach (System.Reflection.MemberInfo m in)
            //        Debug.Log("Variable name is: " + m.Name);
        }
        GUILayout.EndArea();*/



    }


    //Draws the header
    void DrawBaseHeader(Rect rect)
    {
        string name = "Interactable Effect";

        EditorGUI.LabelField(rect, name);
    }

    void DrawConditionalHeader(Rect rect)
    {
        string name = "Conditionals";

        EditorGUI.LabelField(rect, name);
    }

}
#endif