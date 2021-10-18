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
    TowerFactory towerFactory;

    [SerializeField]
    bool setPath = false;
    [SerializeField]
    bool buyChunk = false;

    bool buyTower = false;

    TowerType selectTower;
    public bool basicTowerSelect = true;
    public bool rapidTowerSelect = false;
    public bool quakeTowerSelect = false;
    public bool missleTowerSelect = false;
    // Start is called before the first frame update
    void Start()
    {
        towerFactory = GetComponent<TowerFactory>();
        selectTower = TowerType.BASIC;
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

            if(buyTower)
            {
                BuyTower(edited_tile);
            }
        }
    }

    public void SetBuyChunk(bool trufal)
    {
        gameplayGrid.ResetTileStates();
        buyChunk = trufal;
    }
    public void SetBuildPath(bool trufal)
    {
        gameplayGrid.ResetTileStates();
        setPath = trufal;
        GetDiggableTiles();
    }
    public void SetBuyTower(bool trufal)
    {
        gameplayGrid.ResetTileStates();
        buyTower = trufal;
        ShowBuildTiles();
    }
    // I hate that im doing this but I want to use toggles for this and cant think of another option at the moment
    public void setTower1(bool trufal)
    {
        basicTowerSelect = trufal;
        selectTower = TowerType.BASIC;
    }
    public void setTower2(bool trufal)
    {
        rapidTowerSelect = trufal;
        selectTower = TowerType.RAPID;
    }
    public void setTower3(bool trufal)
    {
        quakeTowerSelect = trufal;
        selectTower = TowerType.QUAKE;
    }
    public void setTower4(bool trufal)
    {
        missleTowerSelect = trufal;
        selectTower = TowerType.MISSLE;
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
            if (neighbour && neighbour.pathfindingState == PathfindingState.NONE && neighbour.interactState != InteractState.UNOWNED && !neighbour.occupied)
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

    private void ShowBuildTiles()
    {
        foreach(Tile tile in gameplayGrid.GetTileList())
        {
            if(tile.interactState != InteractState.UNOWNED && tile.pathfindingState == PathfindingState.NONE)
            {
                tile.interactState = InteractState.GOOD;
                tile.Refresh();
            }
        }
    }

    private void BuyTower(Tile selected_tile)
    {
        if (selected_tile.interactState == InteractState.GOOD)
        {
            GameObject tempTower = towerFactory.CreateTower(selectTower);
            tempTower.transform.position = selected_tile.transform.position;
            selected_tile.occupied = true;
        }
    }
}

