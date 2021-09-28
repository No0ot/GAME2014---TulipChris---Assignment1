using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridChunk : MonoBehaviour
{
    public Vector2 coordinates;

    Tile[] tileList;

    public bool active = false;

    private void Awake()
    {
        tileList = new Tile[Config.chunkSize * Config.chunkSize];
    }

    public void AddTile(int index, Tile tile)
    {
        tileList[index] = tile;
        tile.chunk = this;
        tile.chunkCoordinates = coordinates;
        tile.transform.SetParent(transform, false);
    }

    public void SetActive(bool tempactive)
    {
        active = tempactive;
        foreach(Tile tile in tileList)
        {
            //Color temp = new Color(0f, 0f, 0f, 0f);
            tile.activeMask.gameObject.SetActive(false);
        }
    }
}
