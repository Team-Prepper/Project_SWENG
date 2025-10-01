using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(MapUnitSetter))]
public class TileSetterEditor : Editor {
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("TypeApply"))
        {
            foreach (Object targetObject in targets)
            {
                MapUnitSetter generator = (MapUnitSetter)targetObject;
                generator.SetTile();
            }
        }

        if (GUILayout.Button("EntityApply"))
        {
            foreach (Object targetObject in targets)
            {
                MapUnitSetter generator = (MapUnitSetter)targetObject;
                generator.SetEntity();
            }
        }
    }
}