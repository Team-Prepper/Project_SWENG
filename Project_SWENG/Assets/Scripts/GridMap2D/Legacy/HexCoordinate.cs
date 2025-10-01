/*
using System;
using System.Collections.Generic;
using UnityEngine;
using SWEng.GamePlay;
using SWEng.HexGrid;

public struct HexCoordinate : ICoordinate, IEquatable<HexCoordinate> {

    public int x { get; private set; }
    public int y { get; private set; }

    public HexCoordinate(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public Vector3 ConvertToVector3()
    {
        if (x % 2 == 0)
            return new Vector3(x * HexGridManager.Instance.XOffset, 0, y * HexGridManager.Instance.ZOffset);
        return new Vector3(x * HexGridManager.Instance.XOffset, 0, (y - 0.5f) * HexGridManager.Instance.ZOffset);
    }

    public static IList<HexCoordinate> GetDirectionList(HexCoordinate target)
    {

        if (target.x % 2 == 0)
        {
            return new List<HexCoordinate>
                {
                    new HexCoordinate( target.x,     target.y + 1), //N
                    new HexCoordinate( target.x + 1, target.y + 1), //E1
                    new HexCoordinate( target.x + 1, target.y), //E2
                    new HexCoordinate( target.x,     target.y - 1), //S
                    new HexCoordinate( target.x - 1, target.y), //W1
                    new HexCoordinate( target.x - 1, target.y + 1), //W2
                };
        }

        return new List<HexCoordinate>
            {
                new HexCoordinate( target.x,     target.y + 1), //N
                new HexCoordinate( target.x + 1, target.y), //E1
                new HexCoordinate( target.x + 1, target.y - 1), //E2
                new HexCoordinate( target.x,     target.y - 1), //S
                new HexCoordinate( target.x - 1, target.y - 1), //W1
                new HexCoordinate( target.x - 1, target.y), //W2
            };
    }
    
    public static HexCoordinate ConvertFromVector3(Vector3 source)
    {
        int x = Mathf.RoundToInt(source.x / HexGridManager.Instance.XOffset);
        int z = Mathf.CeilToInt(source.z / HexGridManager.Instance.ZOffset);

        return new HexCoordinate(x, z);
    }

    public bool Equals(HexCoordinate other)
    {
        if (x != other.x) return false;
        if (y != other.y) return false;
        return true;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, y);
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override string ToString()
    {
        return x.ToString() + ", " + y.ToString();
    }

    public static HexCoordinate operator +(HexCoordinate c1, HexCoordinate c2)
    {
        return new HexCoordinate(c1.x + c2.x, c1.y + c2.y);
    }

    public static bool operator ==(HexCoordinate c1, HexCoordinate c2)
    {
        return c1.Equals(c2);
    }

    public static bool operator !=(HexCoordinate c1, HexCoordinate c2)
    {
        if (c1 == null) return true;
        if (c2 == null) return true;
        return !c1.Equals(c2);
    }

    public static ISet<HexCoordinate> GetNeighboursFor(HexCoordinate hexCoordinates, int len) {
        
        ISet<HexCoordinate> retval = new HashSet<HexCoordinate>();

        Vector3 origin = hexCoordinates.ConvertToVector3();

        for (int q = -len; q <= len; q++)
        {
            int r1 = Mathf.Max(-len, -q - len);
            int r2 = Mathf.Min(len, -q + len);

            for (int r = r1; r <= r2; r++)
            {
                float xPos = HexGridManager.Instance.XOffset * q + origin.x;
                float zPos = HexGridManager.Instance.ZOffset * (r + q * 0.5f) + origin.z;
                
                retval.Add(ConvertFromVector3(new Vector3(xPos, 0, zPos)));

            }
        }

        return retval;
    }


}
*/