using UnityEngine;
using System.Collections;
using Photon.Pun;

[SelectionBase]
public class Hex : MonoBehaviour
{

    [SerializeField]
    private GlowHighlight highlight;

    public int Cost {
        get {
            if (Entity == null)
                return _originCost;
            return -1;
        }
    }
    [SerializeField] private int _originCost;

    #region ItemInHex
    
    [Space(20)]
    [Header("Item")]
    private Item item;
    [SerializeField]
    private GameObject itemZone; //parent
    private GameObject itemMesh;
    public Item Item
    {
        get { return item; }
        set
        {
            if (item != value)
            {
                item = value;

                if (value != null)
                {
                    _SpawnItem();
                }
                else
                {
                    Destroy(itemMesh);
                }
            }
        }
    }
    #endregion

    [Header("Cloud")]
    [SerializeField] bool isCloud;
    public bool IsCloud
    {
        get { return isCloud; }
        set 
        {
            if (value == false)
            {
                if (isCloud == false) return;
                isCloud = false;
                StartCoroutine(ActiveFalseCloud());
            }
            return; 
        }
    }
    [SerializeField] private GameObject cloud;

    

    #region Entity

    [Space(20)]
    [Header("Entity")]
    [SerializeField] private GameObject entity;
    public GameObject Entity
    {
        get { return entity; }
        set
        {
            if (entity == value) return;

            entity = value;

            if (value == null) return;

            if (entity.CompareTag("Player"))
            {
                CloudActiveFalse();
            }

            if (item != null && entity.CompareTag("Player"))
            {
                if(entity.GetPhotonView().IsMine == true)
                    _InteractionPlayerWithItem();
            }

            if (tileType == TileDataScript.TileType.village)
            {
                _InteractionPlayerWithShop(entity);
            }
        }
    }

    #endregion

    public TileDataScript.TileType tileType;

    public HexCoordinate HexCoords {
        get {
            return HexCoordinate.ConvertFromVector3(transform.position);
        }
    }

    public bool IsObstacle()
    {
        if(Cost == -1) 
            return true;
        return false;
    }
    private void Awake()
    {
        //int newPosZ = Mathf.RoundToInt(gameObject.transform.position.z * 10);
        //gameObject.transform.position = new Vector3(gameObject.transform.position.x, 0, (float)newPosZ / 10); ;


        highlight = GetComponent<GlowHighlight>();

        HexGrid.Instance.AddTile(this);
        cloud.SetActive(true);
        isCloud = true;

        tileType = GetComponent<HexTileSetter>().type;
        SetCost();
    }

    public void EnableHighlight()
    {
        highlight.ToggleGlow(true);
    }

    public void DisableHighlight()
    {
        highlight.ToggleGlow(false);
    }

    public void OnMouseToggle()
    {
        if (tileType == TileDataScript.TileType.obstacle) return;
        if(highlight)
            highlight.OnMouseToggleGlow();
    }

    internal void ResetHighlight()
    {
        highlight.ResetGlowHighlight();
    }

    internal void HighlightPath()
    {
        highlight.HighlightValidPath();
    }

    private void _InteractionPlayerWithItem()
    {
        item.Pick();
    }

    private void _InteractionPlayerWithShop(GameObject player)
    {
        ShopManager.Instance.WelcomeToShop(player);
    }

    private void _SpawnItem()
    {
        itemMesh = Instantiate(item.itemObject, itemZone.transform);
        //itemMesh.transform.localScale = Vector3.one * 3f;
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


    #region Cloud

    public void CloudActiveFalse()
    {
        foreach (HexCoordinate cloudNeighbours in HexGrid.Instance.GetNeighboursDoubleFor(HexCoords))
        {
            foreach (HexCoordinate cloud in HexGrid.Instance.GetNeighboursFor(cloudNeighbours))
            {
                //StartCoroutine(HexGrid.Instance.GetTileAt(cloud).ActiveFalseCloud());
                HexGrid.Instance.GetTileAt(cloud).IsCloud = false;  
            }

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
    #endregion
}
