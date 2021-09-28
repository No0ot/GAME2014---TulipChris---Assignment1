using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Directions
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}

public enum PathfindingState
{
    NONE,
    START,
    PATH,
    END
}

public class Tile : MonoBehaviour
{
    public Vector2 coordinates;
    public GridChunk chunk;
    public SpriteRenderer activeMask;
    public Vector2 chunkCoordinates;

    public Sprite wallSprite;
    public Sprite pathSprite;

    public PathfindingState pathfindingState = PathfindingState.NONE;

    bool active;

    SpriteRenderer tileSprite;

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
                tileSprite.sprite = pathSprite;
                break;
            case PathfindingState.PATH:
                tileSprite.sprite = pathSprite;
                break;
            case PathfindingState.END:
                tileSprite.sprite = pathSprite;
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
}
