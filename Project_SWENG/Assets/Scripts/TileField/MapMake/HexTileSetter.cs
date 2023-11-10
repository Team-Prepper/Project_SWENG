using LangSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[ExecuteInEditMode]
public class HexTileSetter : MonoBehaviour {

    [SerializeField] TileDataScript.TileType type = TileDataScript.TileType.normal;
    Transform container;
    private void Start()
    {
        SetTile();
    }

    void SetContainer() {

        container = transform.Find("Main");
        if (container == null)
            container = transform;
    }

    public void SetTile() {
        if (container == null)
            SetContainer();
        for (int i = 0; i < container.childCount; i++) {
            DestroyImmediate(container.GetChild(i).gameObject);
        }
        TileDataScript tileData= GridMaker.Instance.GetTileData(type);
        GameObject tile = Instantiate(tileData.tiles[Random.Range(0, tileData.tiles.Length)], transform.position, Quaternion.Euler(0f, Random.Range(0, 6) * 60, 0f));
        tile.layer = LayerMask.NameToLayer("HexTile");

        tile.transform.SetParent(container);
    }
}
