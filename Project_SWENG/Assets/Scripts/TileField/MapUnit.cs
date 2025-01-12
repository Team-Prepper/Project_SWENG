using UnityEngine;
using System.Collections;
using Photon.Pun;
using static UnityEngine.Rendering.DebugUI;

[SelectionBase]
public class MapUnit : MonoBehaviour
{

    [SerializeField] private GlowHighlight highlight;
    [SerializeField] private GameObject cloud;
    [SerializeField] private int _originCost;


    public TileDataScript.TileType tileType;

    public HexCoordinate HexCoords {
        get {
            return HexCoordinate.ConvertFromVector3(transform.position);
        }
    }

    public int Cost => _originCost;

    [Space(20)]
    [Header("Item")]
    [SerializeField] private GameObject _itemZone; //parent
    private GameObject itemMesh;

    public ItemData Item { get; private set; }

    public bool IsCloud { get; private set; }
    public bool IsObstacle => Entity != null;

    public void SetItem(ItemData item) {
        Item = item;

        if (item!= null)
        {
            itemMesh = Instantiate(item.itemObject, _itemZone.transform);
        }
        else
        {
            Destroy(itemMesh);
        }
    }

    #region Entity

    [Space(20)]
    [Header("Entity")]
    [SerializeField] private GameObject _entity;

    public GameObject Entity
    {
        get { return _entity; }
        set
        {
            if (_entity == value) return;

            _entity = value;

            if (value == null) return;

            if (_entity.CompareTag("Player"))
            {
                CloudActiveFalse();
            }

            /*
            if (item != null && _entity.CompareTag("Player"))
            {
                if(_entity.GetPhotonView().IsMine == true)
                    item.Pick();
            }

            if (tileType == TileDataScript.TileType.village)
            {
                ShopManager.Instance.WelcomeToShop(player);
            }*/
        }
    }

    #endregion

    private void Awake()
    {

        HexGrid.Instance.AddTile(this);

        highlight = GetComponent<GlowHighlight>();

        IsCloud = true;
        cloud.SetActive(true);
        
        SetCost();

    }

    public void OnMouseToggle(bool isOn)
    {
        if (tileType == TileDataScript.TileType.obstacle) return;
        if(highlight) highlight.OnMouseToggleGlow(isOn);
    }

    public void SetSprite(Sprite spr, bool isActive) {
        highlight.SetSprite(spr, isActive);
    }

    private void SetCost()
    {
        switch (tileType)
        {
            case TileDataScript.TileType.normal:
                _originCost = 2;
                break;
            case TileDataScript.TileType.hill:
                _originCost = 3;
                break;
            case TileDataScript.TileType.obstacle:
                _originCost = -1;
                break;
            case TileDataScript.TileType.ocean:
                _originCost = -1;
                break;
            case TileDataScript.TileType.village:
                _originCost = 1;
                break;
            case TileDataScript.TileType.castle:
                _originCost = 4;
                break;
            case TileDataScript.TileType.dungon:
                _originCost = 5;
                break;
            default: _originCost = 10; 
                break;
        }
    }

    public void CloudActiveFalse()
    {
        foreach (HexCoordinate cloud in HexGrid.Instance.GetNeighboursFor(HexCoords, 3))
        {
            StartCoroutine(HexGrid.Instance.GetTileAt(cloud).ActiveFalseCloud());
            HexGrid.Instance.GetTileAt(cloud).IsCloud = false;

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
        yield break;
    }
}
