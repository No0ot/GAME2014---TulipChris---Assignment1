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
    public TowerManager towerManager;

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
                if (PlayerStats.Instance.gold >= 20)
                {
                    edited_tile.chunk.SetOwned(true);
                    PlayerStats.Instance.gold -= 20;
                }
                else
                    Debug.Log("Need more gold");
            }

            if(buyTower)
            {
                switch(selectTower)
                {
                    case TowerType.BASIC:
                        if(PlayerStats.Instance.gold >= towerManager.factory.basicTowerPrefab.GetComponent<TowerScript>().goldCost)
                            BuyTower(edited_tile);
                        else
                            Debug.Log("Need more gold");
                        break;
                    case TowerType.RAPID:
                        if (PlayerStats.Instance.gold >= towerManager.factory.rapidTowerPrefab.GetComponent<TowerScript>().goldCost)
                            BuyTower(edited_tile);
                        else
                            Debug.Log("Need more gold");
                        break;
                    case TowerType.QUAKE:
                        if (PlayerStats.Instance.gold >= towerManager.factory.quakeTowerPrefab.GetComponent<TowerScript>().goldCost)
                            BuyTower(edited_tile);
                        else
                            Debug.Log("Need more gold");
                        break;
                    case TowerType.MISSLE:
                        if (PlayerStats.Instance.gold >= towerManager.factory.missleTowerPrefab.GetComponent<TowerScript>().goldCost)
                            BuyTower(edited_tile);
                        else
                            Debug.Log("Need more gold");
                        break;
                }
                
            }

            if(!setPath && !buyChunk && !buyTower && edited_tile.occupied)
            {
                GameplayUIManager.Instance.detailsPanel.gameObject.SetActive(true);
                //GameplayUIManager.Instance.detailsPanel.tower
                GameplayUIManager.Instance.detailsPanel.UpdateTargetReference(edited_tile.occupiedTowerReference, DetailsPanelSetting.UPGRADE);
            }
            else if(!setPath && !buyChunk && !buyTower)
                GameplayUIManager.Instance.detailsPanel.gameObject.SetActive(false);
        }
    }

    public void SetBuyChunk(bool trufal)
    {
        gameplayGrid.ResetTileStates();
        buyChunk = trufal;
    }
    public void SetBuildPath(bool trufal)
    {
        if (trufal)
        {
            gameplayGrid.ResetTileStates();
            setPath = trufal;
            GetDiggableTiles();
        }
        else
        {
            gameplayGrid.ResetTileStates();
            setPath = trufal;
        }
    }
    public void SetBuyTower(bool trufal)
    {
        if (trufal)
        {
            gameplayGrid.ResetTileStates();
            buyTower = trufal;
            ShowBuildTiles();
            GameplayUIManager.Instance.detailsPanel.UpdateTargetReference(selectTower, DetailsPanelSetting.BUILD);
        }
        else
        {
            gameplayGrid.ResetTileStates();
            buyTower = trufal;
        }
    }
    // I hate that im doing this but I want to use toggles for this and cant think of another option at the moment
    public void setTower1(bool trufal)
    {
        basicTowerSelect = trufal;
        selectTower = TowerType.BASIC;
        GameplayUIManager.Instance.detailsPanel.UpdateTargetReference(selectTower, DetailsPanelSetting.BUILD);
    }
    public void setTower2(bool trufal)
    {
        rapidTowerSelect = trufal;
        selectTower = TowerType.RAPID;
        GameplayUIManager.Instance.detailsPanel.UpdateTargetReference(selectTower, DetailsPanelSetting.BUILD);
    }
    public void setTower3(bool trufal)
    {
        quakeTowerSelect = trufal;
        selectTower = TowerType.QUAKE;
        GameplayUIManager.Instance.detailsPanel.UpdateTargetReference(selectTower, DetailsPanelSetting.BUILD);
    }
    public void setTower4(bool trufal)
    {
        missleTowerSelect = trufal;
        selectTower = TowerType.MISSLE;
        GameplayUIManager.Instance.detailsPanel.UpdateTargetReference(selectTower, DetailsPanelSetting.BUILD);
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
            if(tile.interactState != InteractState.UNOWNED && tile.pathfindingState == PathfindingState.NONE && !tile.occupied)
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
            GameObject tempTower = towerManager.GetTower(selectTower);
            tempTower.transform.position = selected_tile.transform.position;
            selected_tile.occupied = true;
            selected_tile.occupiedTowerReference = tempTower;
            gameplayGrid.ResetTileStates();
            ShowBuildTiles();
        }
    }
}

