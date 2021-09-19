using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Grid : MonoBehaviour
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
    }

    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            //if (EventSystem.current.IsPointerOverGameObject())
            //    return;
            HandleInput();
        }
    }

    void HandleInput()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 ray = new Vector2(mousePos.x, mousePos.y);
        RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);

        if(hit.collider != null)
        {
            GetTile(hit.point);
        }
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
        int index = iX + iY * tileCountX + iY / 2;
        Debug.Log("touched at" + coordinates.ToString());
        return tileList[index];
    }
}
