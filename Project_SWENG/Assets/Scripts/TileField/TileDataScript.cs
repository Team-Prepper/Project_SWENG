
using UnityEngine;

[CreateAssetMenu(fileName = "TileData", menuName = "Tile/TileData", order = 2)]

public class TileDataScript : ScriptableObject {
    public enum TileType {
        normal, obstacle, hill, dungon, castle, village, ocean
    }
    public GameObject[] tiles;
    public int cost = 0;
}