using UnityEngine;

[SelectionBase]
public class Hex : MonoBehaviour
{
    [SerializeField]
    private GlowHighlight highlight;

    public static readonly float xOffset = 4.325f, yOffset = 0.5f, zOffset = 5.0f;

    public int Cost {
        get {
            if (Entity == null)
                return _originCost;
            return -1;
        }
    }
    private int _originCost;

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

            if (!value) return;

            if (item != null && entity.CompareTag("Player"))
            {
                _InteractionPlayerWithItem();
            }

            if (tileType == Type.Shop)
            {
                _InteractionPlayerWithShop(entity);
            }
        }
    }

    #endregion
    
    public GameObject tile { set; get; }

    public Vector3Int HexCoords {
        get {
            int x = Mathf.RoundToInt(transform.position.x / xOffset);
            int y = Mathf.RoundToInt(transform.position.y / yOffset);
            int z = Mathf.CeilToInt(transform.position.z / zOffset);

            return new Vector3Int(x, y, z);

        }
    }

    public enum Type {
        Object,
        Shop,
        Obstacle,
        Water,
        Field,
    }
    public Type tileType { get; set; }

    public void WhenCreate(GameObject tile, Transform parent, int cost)
    {
        transform.SetParent(parent);

        this.tile = tile;
        this._originCost = cost;
        HexGrid.Instance.AddTile(this);
        if (tileType == Type.Water)
            tile.transform.localPosition -= new Vector3(0f, 0.6f, 0f);
    }

    public bool IsObstacle()
    {
        if(Cost == -1) 
            return true;
        return false;
    }
    private void OnEnable()
    {
        highlight = GetComponent<GlowHighlight>();
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
        if (tileType == Type.Water) return;
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
        itemMesh.transform.localScale = Vector3.one * 3f;
    }
}
