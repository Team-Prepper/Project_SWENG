
using UnityEngine;

[CreateAssetMenu(fileName = "TileData", menuName = "Tile/TileData", order = 2)]

public class TileDataScript : ScriptableObject {
    public enum TileType {
        normal, rock, hill, dungon, castle, village
    }
    public GameObject[] tiles;
    public int cost = 0;
}