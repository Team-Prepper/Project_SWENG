using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialsConverter {

    static public void ConvertMat(List<GameObject> objTiles)
    {
        // CHK
        Debug.Log("Chg MAT");
        foreach (GameObject objTile in objTiles)
        {
            Transform selectFolder = objTile.transform.Find("Main");
            Renderer renderer = selectFolder.GetComponentInChildren<Renderer>();
            if (renderer == null) continue;

            Material newMat = renderer.material;
            Hex hex = objTile.GetComponent<Hex>();

            foreach (var aroundTile in HexGrid.Instance.GetNeighboursDoubleFor(hex.HexCoords))
            {
                Hex aroundTileHex = HexGrid.Instance.GetTileAt(aroundTile).GetComponent<Hex>();

                if (aroundTileHex == null) continue;
                if (aroundTileHex.tileType == Hex.Type.Object || aroundTileHex.tileType == Hex.Type.Obstacle || aroundTileHex.tileType == Hex.Type.Water) continue;

                Transform selectFolderInAroundTile = aroundTileHex.gameObject.transform.Find("Main");
                Renderer rendererInAroundTile = selectFolderInAroundTile.GetComponentInChildren<Renderer>();
                if (rendererInAroundTile != null)
                {
                    rendererInAroundTile.material = newMat;
                }

            }
        }
    }

}
