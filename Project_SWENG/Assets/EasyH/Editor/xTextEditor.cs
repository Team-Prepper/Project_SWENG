using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(xText))]
public class xTextEditor : Editor {

    public override void OnInspectorGUI()
    {
        if (GUI.changed)
            EditorUtility.SetDirty(target);

        base.OnInspectorGUI();
    }
}