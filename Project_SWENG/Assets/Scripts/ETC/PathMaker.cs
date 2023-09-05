using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection;
using UnityEngine;
using Random = UnityEngine.Random;

public class PathMaker : MonoBehaviour
{
    GridMaker gridMaker;
    [SerializeField] HexGrid hexGrid;
    [SerializeField] GameObject[] tilePath;
    [SerializeField] int costPath = 1;

    private void Awake()
    {
        gridMaker = GetComponentInParent<GridMaker>();
    }
    private void Start()
    {
        //HexGrid.EventSetHexTileDic += SelectPath;
    }

    private void SelectPath(object sender, EventArgs e)
    {
        for(int i = 0; i < 5; i++)
        {
            Hex pathTile = hexGrid.GetRandHex();
            SetPath(pathTile);
        }

        Debug.Log("Make Path Compelete");
        //gridMaker.SetNavMesh();
    }

    void SetPath(Hex pathTile)
    {
        int pathCnt = 0;
        if (pathTile.tileType != Hex.Type.Field) return;

        pathTile.tileType = Hex.Type.Path;
        foreach (var tile in Convert(pathTile.HexCoords))
        {
            if(tile == null) continue;
            if(Random.Range(0, 5) != 0) // 80%
            {
                // Contagion
                SetPath(tile);
            }   
        }


        int rotOffset = 0;
        foreach (var tile in hexGrid.GetNeighboursFor(pathTile.HexCoords))
        {
            
            if (hexGrid.GetTileAt(tile).tileType == Hex.Type.Path)
            {
                pathCnt++;

                Vector3Int diff = tile - pathTile.HexCoords;
                
                switch (diff.x)
                {
                    case -1:
                        if(pathTile.HexCoords.z % 2 == 0)
                            rotOffset = 120;
                        else
                            rotOffset = 60;
                        break;
                    case  0:
                        if (pathTile.HexCoords.z % 2 == 0)
                            rotOffset = -120;
                        else
                            rotOffset = 180;
                        break;
                    case  1:
                        if (pathTile.HexCoords.z % 2 == 0)
                            rotOffset = 0;
                        else
                            rotOffset = -60;
                        break;
                
                }
            }
        }

        ConvertMesh(pathTile, pathCnt, rotOffset);
    }

    private List<Hex> Convert(Vector3Int std)
    {
        List<Hex> result = new List<Hex>();

        if(std.x % 2 == 0)
        {
            for (int i = 1; i <= 5; i += 2)
            {
                result.Add(hexGrid.GetTileAt(std + Direction.GetDirectionList(std.x)[i]));
            }
                
        }
        else
        {
            for (int i = 0; i <= 4; i += 2)
            {
                result.Add(hexGrid.GetTileAt(std + Direction.GetDirectionList(std.x)[i])); 
            }
                
        }

        return result;
    }

    private void ConvertMesh(Hex hextile, int cnt, int rotOffset)
    {
        Vector3 tilePos = hextile.tile.transform.position;
        Destroy(hextile.tile);
        GameObject tile = Instantiate(tilePath[cnt], tilePos, Quaternion.Euler(0f, rotOffset, 0f));
        hextile.cost = costPath;
        hextile.tileType = Hex.Type.Path;

        tile.layer = LayerMask.NameToLayer("HexTile");
        hextile.tile = tile;

        Transform selectFolder = hextile.transform.Find("Main");
        if (selectFolder != null)
            tile.transform.SetParent(selectFolder.transform);
        else
            tile.transform.SetParent(hextile.transform);
    }
}
