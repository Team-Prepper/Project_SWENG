using UnityEditor;
using UnityEditor.UIElements;
using UnityEditor.UI;
using TMPro.EditorUtilities;
using TMPro;
using UnityEngine;
//TMPro.EditorUtilities.TMP_BaseEditorPanel
namespace LangSystem {
    [CustomEditor(typeof(TextMeshPro), true), CanEditMultipleObjects]
    public class xTmpEditor : TMP_EditorPanel {
        static readonly GUIContent k_SortingLayerLabel = new GUIContent("Sorting Layer", "Name of the Renderer's sorting layer.");

        SerializedProperty m_Key;

        protected override void OnEnable()
        {
            m_Key = serializedObject.FindProperty("m_Key");
            base.OnEnable();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            EditorGUILayout.PropertyField(m_Key, k_SortingLayerLabel);
            serializedObject.ApplyModifiedProperties();
        }

    }
}