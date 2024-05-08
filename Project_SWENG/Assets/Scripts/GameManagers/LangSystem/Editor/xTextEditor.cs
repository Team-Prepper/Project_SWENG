using UnityEditor;
using UnityEditor.UIElements;
using UnityEditor.UI;

namespace LangSystem {
    [CustomEditor(typeof(xText))]
    public class xTextEditor : TextEditor {

        SerializedProperty m_Key;

        protected override void OnEnable()
        {
            m_Key = serializedObject.FindProperty("m_Key");
            base.OnEnable();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(m_Key);
            serializedObject.ApplyModifiedProperties();
            base.OnInspectorGUI();
        }
    }
}