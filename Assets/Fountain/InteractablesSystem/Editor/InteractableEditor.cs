using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(Interactable))]
public class InteractableEditor : Editor
{
    SerializedProperty interactableEffects;
    ReorderableList list;

    private void OnEnable()
    {
        interactableEffects = serializedObject.FindProperty("interactableEffects");

        list = new ReorderableList(serializedObject, interactableEffects, true, true, true, true);

        list.drawElementCallback = DrawListItems; 
        list.drawHeaderCallback = DrawHeader; 
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        GUILayout.Space(10);

        list.DoLayoutList();

        if (Application.isPlaying)
        {
            if (GUILayout.Button("Test Fire"))
            {
                ((Interactable)target).Fire();
            }

            if (GUILayout.Button("Reset has fired"))
            {
                ((Interactable)target).hasFired = false;
            }
        }

        serializedObject.ApplyModifiedProperties();
    }


    // Draws the elements on the list
    void DrawListItems(Rect rect, int index, bool isActive, bool isFocused)
    {
        SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index); // The element in the list


        EditorGUI.LabelField(
            new Rect(rect.x, rect.y, 250, EditorGUIUtility.singleLineHeight),
            element.displayName
            );

        EditorGUI.PropertyField(
               new Rect(rect.x + 100, rect.y, 300, EditorGUIUtility.singleLineHeight),               
               element,
               GUIContent.none
           );


    }

    //Draws the header
    void DrawHeader(Rect rect)
    {
        string name = "Interactable Effect Order";
        EditorGUI.LabelField(rect, name);
    }
}
#endif