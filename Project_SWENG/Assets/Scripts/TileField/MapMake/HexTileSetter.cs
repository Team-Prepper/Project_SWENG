using LangSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[ExecuteInEditMode]
public class HexTileSetter : MonoBehaviour {

    public TileDataScript.TileType type = TileDataScript.TileType.normal;

    GridMaker _inforTarget;
    Transform container;

    public void SetInfor(GridMaker inforTarget) {
        _inforTarget = inforTarget;
        SetContainer();
        SetTile();
    }

    void SetContainer() {

        container = transform.Find("Main");
        if (container == null)
            container = transform;
    }

    public void SetTile() {
        for (int i = 0; i < container.childCount; i++) {
            DestroyImmediate(container.GetChild(i).gameObject);
        }
        Vector3 originAngle = transform.eulerAngles;
        transform.eulerAngles = new Vector3(0, 0, 0);

        TileDataScript tileData= _inforTarget.GetTileData(type);
        GameObject tile = Instantiate(tileData.tiles[Random.Range(0, tileData.tiles.Length)], transform.position, Quaternion.Euler(0, Random.Range(0, 6) * 60, 0f));
        tile.layer = LayerMask.NameToLayer("HexTile");

        tile.transform.SetParent(container);
        transform.eulerAngles = originAngle;
    }
}
