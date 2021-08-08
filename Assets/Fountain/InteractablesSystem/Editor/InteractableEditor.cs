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
    SerializedProperty secondaryInteractableEffects;

    ReorderableList baselist;
    ReorderableList secondList;

    private void OnEnable()
    {
        interactableEffects = serializedObject.FindProperty("interactableEffects");
        secondaryInteractableEffects = serializedObject.FindProperty("secondaryInteractableEffects");

        baselist = new ReorderableList(serializedObject, interactableEffects, true, true, true, true);
        secondList = new ReorderableList(serializedObject, secondaryInteractableEffects, true, true, true, true);

        baselist.drawElementCallback = DrawBaseListItems; 
        baselist.drawHeaderCallback = DrawBaseHeader;

        secondList.drawElementCallback = DrawSecondaryListItems;
        secondList.drawHeaderCallback = DrawSecondaryHeader;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        GUILayout.Space(10);

        baselist.DoLayoutList();
        if (((Interactable)target).isFlipFlop)
        {
            secondList.DoLayoutList();
        }

        GUILayout.Space(10);

        if (Application.isPlaying)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label($"DEBUG");
            GUILayout.EndHorizontal();

            GUILayout.Label($"Has Fired: {((Interactable)target).hasFired}");
            GUILayout.Label($"Flip flop State: {((Interactable)target).flipFlopState}");

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
    void DrawBaseListItems(Rect rect, int index, bool isActive, bool isFocused)
    {
        DrawListItems(baselist, rect, index, isActive, isFocused);
    }

    void DrawSecondaryListItems(Rect rect, int index, bool isActive, bool isFocused)
    {
        DrawListItems(secondList, rect, index, isActive, isFocused);
    }

    void DrawListItems(ReorderableList list, Rect rect, int index, bool isActive, bool isFocused)
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
    void DrawBaseHeader(Rect rect)
    {
        string name = "Interactable Effect";

        if (((Interactable)target).isFlipFlop)
            name = "Interactable Effects ( Off -> On )";

        EditorGUI.LabelField(rect, name);
    }

    void DrawSecondaryHeader(Rect rect)
    {
        string name = "Interactable Effect Order";

        if (((Interactable)target).isFlipFlop)
            name = "Interactable Effects ( On -> Off )";

        EditorGUI.LabelField(rect, name);
    }

}
#endif