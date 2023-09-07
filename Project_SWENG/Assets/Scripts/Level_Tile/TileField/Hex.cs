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

    [Space(20)]
    [Header("ITEM")]
    public bool isItem = false;
    public Item item = null;

    //[Space(20)]
    public GameObject tile { set; get; }

    public Vector3Int HexCoords {
        get {
            int x = Mathf.RoundToInt(transform.position.x / xOffset);
            int y = Mathf.RoundToInt(transform.position.y / yOffset);
            int z = Mathf.CeilToInt(transform.position.z / zOffset);

            return new Vector3Int(x, y, z);

        }
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

    private void Awake()
    {
        highlight = GetComponent<GlowHighlight>();

        HexGrid.Instance.AddTile(this);
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

