using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UIElements;

[SelectionBase]
public class Hex : MonoBehaviour
{
    [SerializeField]
    private GlowHighlight highlight;

    public static readonly float xOffset = 4.325f, yOffset = 0.5f, zOffset = 5.0f;

    public int cost = 0;
    private int originCost;

    [Space(20)]
    [Header("Item")]
    public bool isItem = false;
    public Item item = null;

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
                }
                else
                {
                    cost = originCost;
                }
                
            }
        }
    }

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
        Path,
        Obstacle,
        Water,
        Field,
    }
    public Type tileType;

    public int GetCost()
    {
        if(tileType == Type.Path) return 1;
        return cost;
    }

    public bool IsObstacle()
    {
        if(cost == -1) 
            return true;
        return false;
    }

    public void DamageToEntity(int damage)
    {
        if(entity == null) return;

        if (entity.CompareTag("Player"))
        {
            PlayerHealth entityHealth = entity.GetComponent<PlayerHealth>();
            if (entityHealth != null)
            {
                entityHealth.Damaged(damage);
            }
        }
        else
        {
            EnemyController enemy = entity.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.DamagedHandler(damage);
            }
        }
    }

    private void Awake()
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
}

