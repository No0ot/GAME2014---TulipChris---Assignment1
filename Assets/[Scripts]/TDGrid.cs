//      Author          : Chris Tulip
//      StudentID       : 100818050
//      Date Modified   : October 5, 2021
//      File            : TDGrid.cs
//      Description     : Script for the generation and control of the gameplay grid. Taken from https://catlikecoding.com/unity/tutorials/hex-map/ but adapted to square grid.
//      History         :   v0.5 - Creates the grid off of the tile prefab.
//                          v0.7 - While creating tiles adds corresponding tiles to chunks.
//                          v0.9 - Added variables to keep track of the starting tile and the ending tile for enemy pathfinding.
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TDGrid : MonoBehaviour
{
    public Tile tilePrefab;
    public GridChunk chunkPrefab;

    Tile[] tileList;
    GridChunk[] chunkList;

    public Tile startTile;
    public Tile endTile = null;

    int tileCountX;
    int tileCountY;

    public int chunkCountX;
    public int chunkCountY;

    private void Awake()
    {
        Debug.Log(GameManager.Instance.timer);
        tileCountX = chunkCountX * Config.chunkSize;
        tileCountY = chunkCountY * Config.chunkSize;

        CreateChunks();
        CreateTiles();

        GetChunk(new Vector2(2f, 4f)).SetOwned(true);
        startTile = GetTileFromCoordinates(new Vector2(12f, 24f));
        startTile.SetPathfindingState(PathfindingState.START);
        startTile.Refresh();
    }
    /// <summary>
    /// Creates the chunk prefabs that will control and contain the tiles.
    /// </summary>
    void CreateChunks()
    {
        chunkList = new GridChunk[chunkCountX * chunkCountY];

        for(int y = 0, i = 0; y < chunkCountY; y++)
        {
            for(int x = 0; x < chunkCountX; x++)
            {
                GridChunk newChunk = chunkList[i++] = Instantiate(chunkPrefab);
                newChunk.transform.SetParent(transform);
                newChunk.coordinates = new Vector2(x, y);
            }
        }    
    }
    /// <summary>
    /// Creates tiles keeping track of the x and y amounts to later use for the coordinates
    /// </summary>
    void CreateTiles()
    {
        tileList = new Tile[tileCountX * tileCountY];

        for(int y = 0, i = 0; y < tileCountY; y++)
        {
            for(int x  = 0; x < tileCountX; x++)
            {
                CreateTile(x, y, i++);
            }
        }
    }
    /// <summary>
    /// Creates a tile from a prefab and sets the position of the tile along with its coordinates. It then sets the neighbours of the tile and adds the tile to a chunk
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="i"></param>
    void CreateTile(int x, int y, int i)
    {
        Vector2 position;
        position.x = x * Config.tileSize;
        position.y = y * Config.tileSize;

        Tile newTile = tileList[i] = Instantiate<Tile>(tilePrefab);
        newTile.transform.localPosition = position;
        newTile.coordinates = new Vector2(x, y);

        if(x > 0)
        {
            newTile.SetNeighbour(Directions.LEFT, tileList[i - 1]);
        }
        if(y > 0)
        {
            newTile.SetNeighbour(Directions.DOWN, tileList[i - tileCountX]);
        }

        AddTileToChunk(x, y, newTile);
    }
    /// <summary>
    /// Adds passed in tile to a chunk.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="tile"></param>
    void AddTileToChunk(int x, int y, Tile tile)
    {
        int chunkX = x / Config.chunkSize;
        int chunkY = y / Config.chunkSize;
        GridChunk chunk = chunkList[chunkX + chunkY * chunkCountX];

        int localX = x - chunkX * Config.chunkSize;
        int localY = y - chunkY * Config.chunkSize;
        chunk.AddTile(localX + localY * Config.chunkSize, tile);
    }
    /// <summary>
    /// Gets a tile from the tileList based off of a passed in mouse position
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Tile GetTile(Vector2 position)
    {
        position = transform.InverseTransformPoint(position);
        Vector2 coordinates = Config.GetTileFromPosition(position);
        int iX = Mathf.RoundToInt(coordinates.x);
        int iY = Mathf.RoundToInt(coordinates.y);
        int index = iX + iY * 25;
        return tileList[index];
    }
    /// <summary>
    /// Gets a tile using passed in coordinates.
    /// </summary>
    /// <param name="coordinates"></param>
    /// <returns></returns>
    public Tile GetTileFromCoordinates(Vector2 coordinates)
    {
        int iX = Mathf.RoundToInt(coordinates.x);
        int iY = Mathf.RoundToInt(coordinates.y);
        int index = iX + iY * 25;
        return tileList[index];
    }
    /// <summary>
    /// Gets a chunk from passed in coordinates.
    /// </summary>
    /// <param name="coordinate"></param>
    /// <returns></returns>
    public GridChunk GetChunk(Vector2 coordinate)
    {
        int iX = Mathf.RoundToInt(coordinate.x);
        int iY = Mathf.RoundToInt(coordinate.y);
        int index = iX + iY * chunkCountX;
        return chunkList[index];
    }
    /// <summary>
    /// Resets the tile interact states and refreshes them.
    /// </summary>
    public void ResetTileStates()
    {
        foreach(Tile tile in tileList)
        {
            if (tile.interactState != InteractState.UNOWNED)
                tile.interactState = InteractState.NONE;
            tile.Refresh();
        }
    }
    /// <summary>
    /// returns the tileList.
    /// </summary>
    /// <returns></returns>
    public Tile[] GetTileList()
    {
        return tileList;
    }
}
