//      Author          : Chris Tulip
//      StudentID       : 100818050
//      Date Modified   : October 5, 2021
//      File            : GridChunk.cs
//      Description     : Script to contain list of tiles contained in a "chunk". Taken from https://catlikecoding.com/unity/tutorials/hex-map/ but adapted to square grid.
//      History         :   v0.5 - added functionality to add a tile to the list and to change tile Interact state. 
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridChunk : MonoBehaviour
{
    public Vector2 coordinates;

    Tile[] tileList;

    public bool owned = false;

    private void Awake()
    {
        tileList = new Tile[Config.chunkSize * Config.chunkSize];
    }
    /// <summary>
    /// Adds a tile to the list
    /// </summary>
    /// <param name="index"></param>
    /// <param name="tile"></param>
    public void AddTile(int index, Tile tile)
    {
        tileList[index] = tile;
        tile.chunk = this;
        tile.chunkCoordinates = coordinates;
        tile.transform.SetParent(transform, false);
        tile.interactState = InteractState.UNOWNED;
        tile.Refresh();
    }
    /// <summary>
    /// When a chunk is purchases sets all tiles within the chunk to a state that can be interacted with.
    /// </summary>
    /// <param name="tempactive"></param>
    public void SetOwned(bool tempactive)
    {
        owned = tempactive;
        foreach(Tile tile in tileList)
        {
            tile.interactState = InteractState.NONE;
            tile.Refresh();
        }
    }
}
