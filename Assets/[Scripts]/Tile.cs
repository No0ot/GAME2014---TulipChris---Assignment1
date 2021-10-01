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

    public PathfindingState pathfindingState = PathfindingState.NONE;
    public InteractState interactState = InteractState.NONE;

    [SerializeField]
    public Tile[] neighbours;

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

        switch (interactState)
        {
            case InteractState.NONE:
                activeMask.color = new Color(0, 0, 0, 0);
                break;
            case InteractState.GOOD:
                activeMask.color = new Color(0, 255, 0,150);
                break;
            case InteractState.BAD:
                activeMask.color = new Color(255, 0, 0, 150);
                break;
            case InteractState.UNOWNED:
                activeMask.color = new Color(80, 80, 80, 150);
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
