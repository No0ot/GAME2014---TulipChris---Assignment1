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

public class Tile : MonoBehaviour
{
    public Vector2 coordinates;
    public GridChunk chunk;
}
