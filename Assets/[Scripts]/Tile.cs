//      Author          : Chris Tulip
//      StudentID       : 100818050
//      Date Modified   : October 6, 2021
//      File            : Tile.cs
//      Description     : Script for the tiles that keep track of their own neighbours and pathfinding information. Taken from https://catlikecoding.com/unity/tutorials/hex-map/ but adapted to square grid.
//      History         :   v0.5 - Created tile class/Pathfinding State enum and Directions enum. Tiles keep track of their neighbours 
//                          v0.7 - Added Interact state enum and a reference to the next tile along the path(if the tile is part of the path);
//                          v0.9 - Added occupied bool and a reference to the tower that would be occupying the tile.
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Directions
{
    UP,
    LEFT,
    DOWN,
    RIGHT
}

public enum PathfindingState
{
    NONE,
    START,
    PATH,
    END
}

public enum InteractState
{
    NONE,
    GOOD,
    BAD,
    UNOWNED
}

public class Tile : MonoBehaviour
{
    public Vector2 coordinates;
    public GridChunk chunk;
    public SpriteRenderer activeMask;
    public Vector2 chunkCoordinates;

    public Sprite wallSprite;
    public Sprite pathSprite;
    public Sprite startSprite;
    public Sprite endSprite;

    public PathfindingState pathfindingState = PathfindingState.NONE;
    public InteractState interactState = InteractState.NONE;

    [SerializeField]
    public Tile[] neighbours;

    public Tile pathNext;

    public bool occupied;

    SpriteRenderer tileSprite;

    public Color noneColor;
    public Color goodColor;
    public Color badColor;
    public Color unownedColor;

    public GameObject occupiedTowerReference = null;

    private void Awake()
    {
        tileSprite = GetComponent<SpriteRenderer>();
    }
    /// <summary>
    /// Refreshes the tile to update sprite of the tile if a change happens to either the pathfinding state or the interact state.
    /// </summary>
    public void Refresh()
    {
        switch(pathfindingState)
        {
            case PathfindingState.NONE:
                tileSprite.sprite = wallSprite;
                break;
            case PathfindingState.START:
                tileSprite.sprite = startSprite;
                break;
            case PathfindingState.PATH:
                tileSprite.sprite = pathSprite;
                break;
            case PathfindingState.END:
                tileSprite.sprite = endSprite;
                break;
            default:
                break;
        }

        switch (interactState)
        {
            case InteractState.NONE:
                activeMask.color = noneColor;
                break;
            case InteractState.GOOD:
                activeMask.color = goodColor;
                break;
            case InteractState.BAD:
                activeMask.color = badColor;
                break;
            case InteractState.UNOWNED:
                activeMask.color = unownedColor;
                break;
            default:
                break;
        }
    }

    public void SetPathfindingState(PathfindingState state)
    {
        pathfindingState = state;
        Refresh();
    }
    /// <summary>
    /// Returns a reference to the neighbour in the passed in direction
    /// </summary>
    /// <param name="neighbour_direction"></param>
    /// <returns></returns>
    public Tile GetNeighbour(Directions neighbour_direction)
    {
        return neighbours[(int)neighbour_direction];
    }
    /// <summary>
    /// Sets the neighbour in the passed in direction along with the opposite neighbour of the direction
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="tile"></param>
    public void SetNeighbour(Directions direction, Tile tile)
    {
        neighbours[(int)direction] = tile;
        tile.neighbours[(int)GetOppositeNeighbour(direction)] = this;
    }
    /// <summary>
    /// returns a Direction(enum) that is opposite to the passed in direction
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public Directions GetOppositeNeighbour(Directions direction)
    {
        return (int)direction < 2 ? (direction + 2) : (direction - 2);
    }
}
