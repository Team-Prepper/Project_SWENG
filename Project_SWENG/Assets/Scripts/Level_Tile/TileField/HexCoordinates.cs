using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCoordinates : MonoBehaviour
{
    public static float xOffset = 4.325f, zOffset = 5.0f; //  yOffset = 0.5f,

    internal Vector3Int GetHexCoords()
        => offsetCoordinates;

    [SerializeField]
    private Vector3Int offsetCoordinates;

    private void Awake()
    {
        offsetCoordinates = ConvertPositionToOffset(transform.position);
    }

    public static Vector3Int ConvertPositionToOffset(Vector3 position)
    {
        int x = Mathf.RoundToInt(position.x / xOffset);
        int y = 0;
        int z = Mathf.CeilToInt(position.z / zOffset);
        return new Vector3Int(x, y, z);
    }
}
