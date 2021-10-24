//      Author          : Chris Tulip
//      StudentID       : 100818050
//      Date Modified   : October 24, 2021
//      File            : GridEditor.cs
//      Description     : This script is waht controls any editing to the grid. Main script for the player interacting with the game.
//                        Parts of it were taken from: https://catlikecoding.com/unity/tutorials/hex-map/
//      History         :   v0.2 - tiles can be accessed using mouse or touch
//                          v0.5 - Functionality for digging the maze added along with buying additional "chunks"
//                          v0.7 - Functionality for building towers added.
//                          v1.0 - Functionality for detail panel added which allows players to see more detailed information about towers prior to building along with upgrade panel for alrady built towers.
//
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
    /// <summary>
    /// Handles Input, Checks if the touch/pointer is over a UI object if not and the raycast hits a tile, edits the tile.
    /// </summary>
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
    /// <summary>
    /// Edits the tile based on the currently active Bool. Can dig out the maze, buy additional Chunks, buy/place towers and access details about placed towers
    /// </summary>
    /// <param name="edited_tile"></param>
    void EditTile(Tile edited_tile)
    {
        if(edited_tile)
        {
            //Dig path call
            if(setPath && edited_tile.chunk.owned)
            {
                DigTile(edited_tile);
            }
            //Buy chunk call
            if(buyChunk)
            {
                if (PlayerStats.Instance.gold >= 20)
                {
                    if (!edited_tile.chunk.owned)
                    {
                        edited_tile.chunk.SetOwned(true);
                        PlayerStats.Instance.gold -= 20;
                    }
                    else
                        GameplayUIManager.Instance.DisplayErrorText("Already owned Chunk!");
                }
                else
                    GameplayUIManager.Instance.DisplayErrorText("Need more gold!");
            }
            //Buy tower call
            if(buyTower)
            {
                switch(selectTower)
                {
                    case TowerType.BASIC:
                        if(PlayerStats.Instance.gold >= towerManager.factory.basicTowerPrefab.GetComponent<TowerScript>().goldCost)
                            BuyTower(edited_tile);
                        else
                            GameplayUIManager.Instance.DisplayErrorText("Need more gold!");
                        break;
                    case TowerType.RAPID:
                        if (PlayerStats.Instance.gold >= towerManager.factory.rapidTowerPrefab.GetComponent<TowerScript>().goldCost)
                            BuyTower(edited_tile);
                        else
                            GameplayUIManager.Instance.DisplayErrorText("Need more gold!");
                        break;
                    case TowerType.QUAKE:
                        if (PlayerStats.Instance.gold >= towerManager.factory.quakeTowerPrefab.GetComponent<TowerScript>().goldCost)
                            BuyTower(edited_tile);
                        else
                            GameplayUIManager.Instance.DisplayErrorText("Need more gold!");
                        break;
                    case TowerType.MISSLE:
                        if (PlayerStats.Instance.gold >= towerManager.factory.missleTowerPrefab.GetComponent<TowerScript>().goldCost)
                            BuyTower(edited_tile);
                        else
                            GameplayUIManager.Instance.DisplayErrorText("Need more gold!");
                        break;
                }
                
            }
            // if selected tile has a tower occupying it, passes in a reference of the tower to the details panel
            if (!setPath && !buyChunk && !buyTower && edited_tile.occupied)
            {
                GameplayUIManager.Instance.detailsPanel.gameObject.SetActive(true);
                GameplayUIManager.Instance.detailsPanel.UpdateTargetReference(edited_tile.occupiedTowerReference, DetailsPanelSetting.UPGRADE);
            }
            // If a tile with no tower is selected, removes the detail panel.
            else if (!setPath && !buyChunk && !buyTower)
            {
                GameplayUIManager.Instance.detailsPanel.gameObject.SetActive(false);
            }
        }
    }
    /// <summary>
    /// Toggle function for setting buyChunk bool
    /// </summary>
    /// <param name="trufal"></param>
    public void SetBuyChunk(bool trufal)
    {
        SoundManager.Instance.PlayRandomClickForward();
        gameplayGrid.ResetTileStates();
        buyChunk = trufal;
    }
    /// <summary>
    /// Toggle function for setting setPath bool
    /// </summary>
    /// <param name="trufal"></param>
    public void SetBuildPath(bool trufal)
    {
        SoundManager.Instance.PlayRandomClickForward();
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
    /// <summary>
    /// Toggle function for buyTower bool, also brings up detail panel
    /// </summary>
    /// <param name="trufal"></param>
    public void SetBuyTower(bool trufal)
    {
        SoundManager.Instance.PlayRandomClickForward();
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

    // I hate that im doing this but I want to use toggles for this and cant think of another option at the moment.
    /// <summary>
    /// Sets selectTower to Basic tower.
    /// </summary>
    /// <param name="trufal"></param>
    public void setTower1(bool trufal)
    {
        SoundManager.Instance.PlayRandomClickForward();
        basicTowerSelect = trufal;
        selectTower = TowerType.BASIC;
        GameplayUIManager.Instance.detailsPanel.UpdateTargetReference(selectTower, DetailsPanelSetting.BUILD);
    }
    /// <summary>
    /// sets SelectTower to Rapid tower
    /// </summary>
    /// <param name="trufal"></param>
    public void setTower2(bool trufal)
    {
        SoundManager.Instance.PlayRandomClickForward();
        rapidTowerSelect = trufal;
        selectTower = TowerType.RAPID;
        GameplayUIManager.Instance.detailsPanel.UpdateTargetReference(selectTower, DetailsPanelSetting.BUILD);
    }
    /// <summary>
    /// sets SelectTower to Quake tower(not implemented)
    /// </summary>
    /// <param name="trufal"></param>
    public void setTower3(bool trufal)
    {
        SoundManager.Instance.PlayRandomClickForward();
        quakeTowerSelect = trufal;
        selectTower = TowerType.QUAKE;
        GameplayUIManager.Instance.detailsPanel.UpdateTargetReference(selectTower, DetailsPanelSetting.BUILD);
    }
    /// <summary>
    /// sets selectTower to Missle tower(not implemented)
    /// </summary>
    /// <param name="trufal"></param>
    public void setTower4(bool trufal)
    {
        SoundManager.Instance.PlayRandomClickForward();
        missleTowerSelect = trufal;
        selectTower = TowerType.MISSLE;
        GameplayUIManager.Instance.detailsPanel.UpdateTargetReference(selectTower, DetailsPanelSetting.BUILD);
    }
    /// <summary>
    /// //EventSystem.current.IsPointerOverGameObject() was not working properly so i did some research and found this function : https://stackoverflow.com/questions/57010713/unity-ispointerovergameobject-issue
    /// </summary>
    /// <returns></returns>
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
    /// <summary>
    /// Function to get the diggable triles, passes in the end tile to build off of. If end tile doesnt exist passes in the start tile.
    /// </summary>
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
    /// <summary>
    /// Shows the tiles eligble to be dug out for the maze. To be eligble the tile has to be next to the passed in tile and none of its neighbours can be next to another tile of path.
    /// </summary>
    /// <param name="firstile"></param>
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
    /// <summary>
    /// Sets the tile state to path if it is elible gets a reference to the tile being built off of for enemy pathfinding.
    /// </summary>
    /// <param name="selected_tile"></param>
    private void DigTile(Tile selected_tile)
    {
        if(gameplayGrid.endTile == null)
        {
            if (selected_tile.interactState == InteractState.GOOD)
            {
                SoundManager.Instance.PlayDigSound();
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
                SoundManager.Instance.PlayDigSound();
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
    /// <summary>
    /// Shows tiles elible to be built on.
    /// </summary>
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
    /// <summary>
    /// Places the selceted tower on the tile that is passed in.
    /// </summary>
    /// <param name="selected_tile"></param>
    private void BuyTower(Tile selected_tile)
    {
        if (selected_tile.interactState == InteractState.GOOD)
        {
            SoundManager.Instance.PlayTowerBuild();
            GameObject tempTower = towerManager.GetTower(selectTower);
            tempTower.transform.position = selected_tile.transform.position;
            selected_tile.occupied = true;
            selected_tile.occupiedTowerReference = tempTower;
            gameplayGrid.ResetTileStates();
            ShowBuildTiles();
        }
    }
}

