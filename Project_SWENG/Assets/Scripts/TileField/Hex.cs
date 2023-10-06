using UnityEngine;

[SelectionBase]
public class Hex : MonoBehaviour
{
    [SerializeField]
    private GlowHighlight highlight;

    public static readonly float xOffset = 4.325f, yOffset = 0.5f, zOffset = 5.0f;

    public int cost = 0;
    private int originCost;

    #region ItemInHex
    
    [Space(20)]
    [Header("Item")]
    private Item item;
    [SerializeField] private GameObject itemZone; //parent
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
                    SpawnItem();
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
    private GameObject entity;
    public GameObject Entity
    {
        get { return entity; }
        set
        {
            if (entity != value)
            {
                entity = value;

                if (value != null)
                {
                    cost = -1;

                    if (item != null && entity.CompareTag("Player"))
                    {
                        InteractionPlayerWithItem();
                    }

                    if(tileType == Type.Shop)
                    {
                        InteractionPlayerWithShop();
                    }

                }
                else
                {
                    cost = originCost;
                }
                
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

    public void WhenCreate(GameObject tile, Transform parent, int cost)
    {
        this.tile = tile;
        transform.SetParent(parent);
        this.cost = cost;
        this.originCost = cost;
        HexGrid.Instance.AddTile(this);
    }

    public enum Type
    {
        Object,
        Shop,
        Obstacle,
        Water,
        Field,
    }
    public Type tileType;

    public int GetCost()
    {
        return cost;
    }

    public bool IsObstacle()
    {
        if(cost == -1) 
            return true;
        return false;
    }
    private void Start()
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

    private void InteractionPlayerWithItem()
    {
        item.Pick();
    }

    private void InteractionPlayerWithShop()
    {
        ShopManager.Instance.WelcomeToShop();
    }

    private void SpawnItem()
    {
        itemMesh = Instantiate(item.itemObject, itemZone.transform);
        itemMesh.transform.localScale = Vector3.one * 3f;
    }
}
