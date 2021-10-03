//********GAME2014 - MOBILE GAME DEV ASSIGNMENT 1*****************
// CHRIS TULIP 100 818 050
//
// A config class to store const variables and static functions.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Config
{
    public const float tileSize = 1.28f;

    public const int chunkSize = 5;

    public const float gridOffsetX = 10;
    public const float gridOffsetY = 5;

    public static Vector2 GetTileFromPosition(Vector2 position)
    {
        float x = position.x / tileSize;
        float y = position.y / tileSize;

        int iX = Mathf.RoundToInt(x);
        int iY = Mathf.RoundToInt(y);
        return new Vector2(iX, iY);
    }
}
