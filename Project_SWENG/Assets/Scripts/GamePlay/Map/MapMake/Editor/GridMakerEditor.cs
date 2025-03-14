using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapMaker))]
public class GridMakerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MapMaker generator = (MapMaker)target;

        if (GUILayout.Button("Generate World")) {
            generator.CreateHexGrid();
        }
        if (GUILayout.Button("EndEdit")) {
            generator.EndEdit();
        }
        if (GUILayout.Button("RemoveAll"))
        {
            generator.RemoveAll();
        }
    }
}
