using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(StateInteractable))]
public class StateInteractableEditor : Editor
{
    SerializedProperty interactableEffects;
    
    List<ReorderableList> lists;

    private void OnEnable()
    {
        interactableEffects = serializedObject.FindProperty("interactableEffects");

        RefreshLists();
    }

    private void RefreshLists()
    {
        lists = new List<ReorderableList>();

        for (int i = 0; i < interactableEffects.arraySize; i++)
        {
            int listIndex = i;
            var objectProperty = interactableEffects.GetArrayElementAtIndex(listIndex);
            var objectList = objectProperty.FindPropertyRelative("stateInteractableEffects");

            ReorderableList newList = new ReorderableList(serializedObject, objectList, true, true, true, true);
            newList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                DrawListItems(listIndex, rect, index, isActive, isFocused);
            };

            newList.drawHeaderCallback = (Rect rect) =>
            {
                DrawListHeader(listIndex, rect);
            };
                      
            lists.Add(newList);
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        GUILayout.Space(10);

        if (interactableEffects.arraySize != lists.Count)
        {
            RefreshLists();
        }


        GUILayout.BeginVertical(EditorStyles.helpBox);

        for (int i = 0; i < interactableEffects.arraySize; i++)
        {
            lists[i].DoLayoutList();
        }
        serializedObject.ApplyModifiedProperties();


        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add State"))
        {
            interactableEffects.arraySize++;
            serializedObject.ApplyModifiedProperties();

        }
        if (GUILayout.Button("Remove State"))
        {
            if (interactableEffects.arraySize > 0)
            {
                interactableEffects.arraySize--;
                serializedObject.ApplyModifiedProperties();
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        if (Application.isPlaying)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label($"DEBUG");
            GUILayout.EndHorizontal();

            GUILayout.Label($"Current State: {((StateInteractable)target).curState}");

            if (GUILayout.Button("Test Fire"))
            {
                ((StateInteractable)target).Fire();
            }
        }
    }

    void DrawListItems(int listIndex, Rect rect, int index, bool isActive, bool isFocused)
    {
        ReorderableList list = lists[listIndex];
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

    void DrawListHeader(int listIndex, Rect rect)
    {
        string name = $"State {listIndex} Interactable Effects ";

        EditorGUI.LabelField(rect, name);
    }


}

#endif