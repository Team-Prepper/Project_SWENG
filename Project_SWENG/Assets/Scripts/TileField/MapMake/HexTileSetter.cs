using EHTool.LangKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[ExecuteInEditMode]
public class HexTileSetter : MonoBehaviour {

    public TileDataScript.TileType type = TileDataScript.TileType.normal;

    [SerializeField] GameObject _entity;

    MapMaker _inforTarget;
    Transform _container;

    public void SetInfor(MapMaker inforTarget) {
        _inforTarget = inforTarget;
        SetContainer();
        SetTile();
    }

    void SetContainer() {

        _container = transform.Find("Main");
        if (_container == null)
            _container = transform;
    }

    public void SetTile() {

        for (int i = 0; i < _container.childCount; i++) {
            DestroyImmediate(_container.GetChild(i).gameObject);
        }

        Vector3 originAngle = transform.eulerAngles;
        transform.eulerAngles = new Vector3(0, 0, 0);

        TileDataScript tileData= _inforTarget.GetTileData(type);
        GameObject tile = Instantiate(tileData.tiles[Random.Range(0, tileData.tiles.Length)], transform.position, Quaternion.Euler(0, Random.Range(0, 6) * 60, 0f));
        tile.layer = LayerMask.NameToLayer("HexTile");

        tile.transform.SetParent(_container);
        transform.eulerAngles = originAngle;

    }

    public void SetEntity() {
        GameObject entity = Instantiate(_entity, _container);
        entity.transform.localPosition = Vector3.zero;
        gameObject.GetComponent<MapUnit>().Entity = entity;
    }

}
