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

    int tileCountX;
    int tileCountY;

    public int chunkCountX;
    public int chunkCountY;

    private void Awake()
    {
        tileCountX = chunkCountX * Config.chunkSize;
        tileCountY = chunkCountY * Config.chunkSize;

        CreateChunks();
        CreateTiles();
        GetChunk(new Vector2(2f, 4f)).SetActive(true);
        Tile starttile = GetTileFromCoordinates(new Vector2(12f, 24f));
        starttile.SetPathfindingState(PathfindingState.START);
        starttile.Refresh();
    }

    private void Update()
    {
       
    }

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

    void CreateTile(int x, int y, int i)
    {
        Vector2 position;
        position.x = x * Config.tileSize;
        position.y = y * Config.tileSize;
        //position.x = (x - Config.gridOffsetX) * Config.tileSize;
        //position.y = (y - Config.gridOffsetY) * Config.tileSize;

        Tile newTile = tileList[i] = Instantiate<Tile>(tilePrefab);
        newTile.transform.localPosition = position;
        newTile.coordinates = new Vector2(x, y);

        AddTileToChunk(x, y, newTile);
    }

    void AddTileToChunk(int x, int y, Tile tile)
    {
        int chunkX = x / Config.chunkSize;
        int chunkY = y / Config.chunkSize;
        GridChunk chunk = chunkList[chunkX + chunkY * chunkCountX];

        int localX = x - chunkX * Config.chunkSize;
        int localY = y - chunkY * Config.chunkSize;
        chunk.AddTile(localX + localY * Config.chunkSize, tile);
    }

    public Tile GetTile(Vector2 position)
    {
        position = transform.InverseTransformPoint(position);
        Vector2 coordinates = Config.GetTileFromPosition(position);
        int iX = Mathf.RoundToInt(coordinates.x);
        int iY = Mathf.RoundToInt(coordinates.y);
        int index = iX + iY * 25;
        return tileList[index];
    }

    public Tile GetTileFromCoordinates(Vector2 coordinates)
    {
        int iX = Mathf.RoundToInt(coordinates.x);
        int iY = Mathf.RoundToInt(coordinates.y);
        int index = iX + iY * 25;
        return tileList[index];
    }

    public GridChunk GetChunk(Vector2 coordinate)
    {
        int iX = Mathf.RoundToInt(coordinate.x);
        int iY = Mathf.RoundToInt(coordinate.y);
        int index = iX + iY * chunkCountX;
        return chunkList[index];
    }
}