using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[SelectionBase]
public class Hex : MonoBehaviour
{
    [SerializeField]
    private GlowHighlight highlight;
    private HexCoordinates hexCoordinates;

    public int cost = 0;

    [Space(20)]
    [Header("ITEM")]
    public bool isItem = false;
    public Item item = null;

    [Space(20)]
    public GameObject tile;
    public Vector3Int HexCoords => hexCoordinates.GetHexCoords();

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
        hexCoordinates = GetComponent<HexCoordinates>();
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

