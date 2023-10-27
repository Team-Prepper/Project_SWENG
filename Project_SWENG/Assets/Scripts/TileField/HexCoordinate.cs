using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public struct HexCoordinate : IEquatable<HexCoordinate> {

    public static readonly float xOffset = 4.325f;
    public static readonly float zOffset = 5.0f;

    public int x;
    public int z;

    public HexCoordinate (int x, int z) {
        this.x = x;
        this.z = z;
    }

    public Vector3 ConvertToVector3() {
        return new Vector3();
    }

    public static HexCoordinate ConvertFromVector3(Vector3 source)
    {
        int x = Mathf.RoundToInt(source.x / xOffset);
        int z = Mathf.CeilToInt(source.z / zOffset);

        return new HexCoordinate(x, z);
    }

    public bool Equals(HexCoordinate other)
    {
        if (x != other.x) return false;
        if (z != other.z) return false;
        return true;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, z);
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override string ToString()
    {
        return x.ToString() + ", " + z.ToString();
    }

    public static HexCoordinate operator +(HexCoordinate c1, HexCoordinate c2)
    {
        return new HexCoordinate(c1.x + c2.x, c1.z + c2.z);
    }

    public static bool operator ==(HexCoordinate c1, HexCoordinate c2)
    {
        return c1.Equals(c2);
    }

    public static bool operator !=(HexCoordinate c1, HexCoordinate c2) {
        if (c1 == null) return true;
        if (c2 == null) return true;
        return !c1.Equals(c2);
    }

}
