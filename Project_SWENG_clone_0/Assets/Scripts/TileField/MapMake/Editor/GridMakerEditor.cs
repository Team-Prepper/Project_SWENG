using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridMaker))]
public class GridMakerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GridMaker generator = (GridMaker)target;

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
