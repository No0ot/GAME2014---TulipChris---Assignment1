//********GAME2014 - MOBILE GAME DEV ASSIGNMENT 1*****************
// CHRIS TULIP 100 818 050
//
// A script for Tile prefabs to handle their behaviours. Also includes the enums used.
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

    bool active;

    SpriteRenderer tileSprite;

    public Color noneColor;
    public Color goodColor;
    public Color badColor;
    public Color unownedColor;

    private void Awake()
    {
        tileSprite = GetComponent<SpriteRenderer>();
    }
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

    public Tile GetNeighbour(Directions neighbour_direction)
    {
        return neighbours[(int)neighbour_direction];
    }

    public void SetNeighbour(Directions direction, Tile tile)
    {
        neighbours[(int)direction] = tile;
        tile.neighbours[(int)GetOppositeNeighbour(direction)] = this;
    }

    public Directions GetOppositeNeighbour(Directions direction)
    {
        return (int)direction < 2 ? (direction + 2) : (direction - 2);
    }
}
