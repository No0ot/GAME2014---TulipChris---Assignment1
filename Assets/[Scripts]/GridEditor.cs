//********GAME2014 - MOBILE GAME DEV ASSIGNMENT 1*****************
// CHRIS TULIP 100 818 050
//
// A script that handles any of the functions that make changes to the grid. It is basically the player controller.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridEditor : MonoBehaviour
{
    public TDGrid gameplayGrid;

    [SerializeField]
    bool setPath = false;
    [SerializeField]
    bool buyChunk = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
                HandleInput();
        }
    }

    void HandleInput()
    {
        if (IsPointerOverUIObject())
            return;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 ray = new Vector2(mousePos.x, mousePos.y);
        RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);

        if (hit.collider != null)
        {
            EditTile(gameplayGrid.GetTile(hit.point));
        }
    }

    void EditTile(Tile edited_tile)
    {
        if(edited_tile)
        {
            if(setPath && edited_tile.chunk.owned)
            {
                DigTile(edited_tile);
            }

            if(buyChunk)
            {
                edited_tile.chunk.SetOwned(true);
            }
        }
    }

    public void SetBuyChunk(bool trufal)
    {
        buyChunk = trufal;
    }
    public void SetBuildPath(bool trufal)
    {
        setPath = trufal;
        GetDiggableTiles();
    }

    //EventSystem.current.IsPointerOverGameObject() was not working properly so i did some research and found this function : https://stackoverflow.com/questions/57010713/unity-ispointerovergameobject-issue
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    private void GetDiggableTiles()
    {
       if(gameplayGrid.endTile == null)
       {
            ShowDigTiles(gameplayGrid.startTile);
       }
       else
       {
            ShowDigTiles(gameplayGrid.endTile);
       }
    }

    private void ShowDigTiles(Tile firstile)
    {
        Tile[] neighbours = firstile.neighbours;

        foreach (Tile neighbour in neighbours)
        {
            if (neighbour && neighbour.pathfindingState == PathfindingState.NONE && neighbour.interactState != InteractState.UNOWNED)
            {
                Tile[] neighbours2 = neighbour.neighbours;
                bool canshow = true;

                foreach (Tile neighbour2 in neighbours2)
                {
                    if (neighbour2)
                    {
                        if (neighbour2 == firstile)
                            continue;
                        if(neighbour2.pathfindingState != PathfindingState.NONE)
                            canshow = false;   

                    }
                }
                if (canshow)
                {
                    neighbour.interactState = InteractState.GOOD;
                    neighbour.Refresh();
                }
            }
            else
                continue;
        }
    }

    private void DigTile(Tile selected_tile)
    {
        if(gameplayGrid.endTile == null)
        {
            if (selected_tile.interactState == InteractState.GOOD)
            {
                selected_tile.SetPathfindingState(PathfindingState.END);
                gameplayGrid.startTile.pathNext = selected_tile;
                gameplayGrid.endTile = selected_tile;
                gameplayGrid.ResetTileStates();
                if (setPath)
                    GetDiggableTiles();
            }
        }
        else
        {
            if (selected_tile.interactState == InteractState.GOOD)
            {
                selected_tile.SetPathfindingState(PathfindingState.END);
                gameplayGrid.endTile.pathNext = selected_tile;
                gameplayGrid.endTile.SetPathfindingState(PathfindingState.PATH);
                gameplayGrid.endTile = selected_tile;

                gameplayGrid.ResetTileStates();
                if (setPath)
                    GetDiggableTiles();
            }
        }
    }
}

