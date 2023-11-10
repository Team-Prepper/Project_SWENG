using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HexTileSetter))]
public class TileSetterEditor : Editor {
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        HexTileSetter generator = (HexTileSetter)target;

        if (GUILayout.Button("Apply"))
        {
            generator.SetTile();
        }
    }
}