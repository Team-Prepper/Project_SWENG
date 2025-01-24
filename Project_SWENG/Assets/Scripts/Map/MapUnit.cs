using UnityEngine;
using System.Collections;
using EHTool.UIKit;
using static UnityEngine.EventSystems.EventTrigger;

[SelectionBase]
public class MapUnit : MonoBehaviour {

    [SerializeField] private GlowHighlight highlight;
    [SerializeField] private GameObject cloud;

    [SerializeField] private int _originCost;
    public int Cost => _originCost;

    public TileDataScript.TileType tileType;

    public HexCoordinate HexCoords => HexCoordinate.ConvertFromVector3(transform.position);

    [SerializeField] private GameObject _entity;

    private ICharacterController _cc;
    private IItemController _ic;

    public GameObject Entity => _entity;
    public ICharacterController CC => _cc;
    public bool IsCloud { get; private set; } = true;

    public void ResetEntityState()
    {
        _entity = null;
        _cc = null;
        _ic = null;
    }

    public void SetCC(GameObject entity, ICharacterController cc = null) {

        if (_entity == entity) return;
        _entity = entity;

        if (entity == null) _cc = null;

        _cc = cc;

        if (CC.TeamIdx == 0)
        {
            CloudActiveFalse();
        }

    }

    public void SetItem(GameObject entity, IItemController ic) {
        _entity = entity;
        _ic = ic;
    }

    public bool IsObstacle {
        get {
            if (Entity != null) return true;
            if (tileType == TileDataScript.TileType.obstacle) return true;
            if (tileType == TileDataScript.TileType.village) return true;
            return false;
        }
    }

    [SerializeField] private GameObject _itemZone; //parent
    private GameObject itemMesh;

    private void Start()
    {
        highlight = GetComponent<GlowHighlight>();

        IsCloud = true;
        cloud.SetActive(true);

    }

    public void OnMouseToggle(bool isOn)
    {
        if (tileType == TileDataScript.TileType.obstacle) return;
        if(highlight) highlight.OnMouseToggleGlow(isOn);
    }

    public void SetSprite(Sprite spr, Vector3 localScale, Vector3 eulerAngle, bool isActive) {
        highlight.SetSprite(spr, localScale, eulerAngle, isActive);
    }

    public void SetCost(int cost) {
        _originCost = cost;
    }

    public void CloudActiveFalse()
    {
        foreach (HexCoordinate cloud in HexGrid.Instance.GetNeighboursFor(HexCoords, 3))
        {
            if (!HexGrid.Instance.GetMapUnitAt(cloud).IsCloud) continue;
            StartCoroutine(HexGrid.Instance.GetMapUnitAt(cloud).ActiveFalseCloud());

        }
    }

    private IEnumerator ActiveFalseCloud()
    {
        for (int i = 0; i < 10; i++)
        {
            cloud.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
            yield return new WaitForSeconds(0.05f);
        }
        cloud.SetActive(false);
        IsCloud = false;
        yield break;
    }

    public void Interaction(ICharacterController actor)
    {
        if (tileType == TileDataScript.TileType.village)
        {
            UIManager.Instance.OpenGUI<GUIShop>("Shop").SetCC(actor, this);
            return;
        }
        if (_cc != null) {
            UIManager.Instance.OpenGUI<GUI_ShowCharacterInfor>("CharacterInfor").SetInfor(_cc, actor);
            return;
        }
        if (_ic != null) {
            _ic.Interaction(actor);
        }

        actor.ActionEnd(0);

    }
}
