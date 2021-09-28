using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridEditor : MonoBehaviour
{
    public TDGrid gameplayGrid;

    bool setPath = true;

    bool buyChunk = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
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

        if (hit.collider != null)
        {
            EditTile(gameplayGrid.GetTile(hit.point));
        }
    }

    void EditTile(Tile edited_tile)
    {
        if(edited_tile)
        {
            if(setPath && edited_tile.chunk.active)
            {
                if(edited_tile.pathfindingState == PathfindingState.NONE)
                    edited_tile.SetPathfindingState(PathfindingState.PATH);
                else if(edited_tile.pathfindingState == PathfindingState.PATH)
                    edited_tile.SetPathfindingState(PathfindingState.NONE);
            }

            if(buyChunk)
            {
                edited_tile.chunk.SetActive(true);
            }
        }
    }
}
