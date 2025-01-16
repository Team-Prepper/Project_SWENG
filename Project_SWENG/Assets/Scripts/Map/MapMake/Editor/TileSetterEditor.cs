using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(HexTileSetter))]
public class TileSetterEditor : Editor {
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("TypeApply"))
        {
            foreach (Object targetObject in targets)
            {
                HexTileSetter generator = (HexTileSetter)targetObject;
                generator.SetTile();
            }
        }

        if (GUILayout.Button("EntityApply"))
        {
            foreach (Object targetObject in targets)
            {
                HexTileSetter generator = (HexTileSetter)targetObject;
                generator.SetEntity();
            }
        }
    }
}