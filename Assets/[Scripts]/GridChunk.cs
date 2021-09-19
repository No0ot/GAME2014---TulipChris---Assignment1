using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridChunk : MonoBehaviour
{
    public Vector2 coordinates;

    Tile[] tileList;

    bool active;

    private void Awake()
    {
        tileList = new Tile[Config.chunkSize * Config.chunkSize];
    }

    public void AddTile(int index, Tile tile)
    {
        tileList[index] = tile;
        tile.chunk = this;
        tile.transform.SetParent(transform, false);
    }
}
