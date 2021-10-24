//      Author          : Chris Tulip
//      StudentID       : 100818050
//      Date Modified   : September 28, 2021
//      File            : Config.cs
//      Description     : This script contains a static class for holding global static variables mostly dealing with grid properties
//      History         :   v0.5 - Added static properties the grid can access along with function to get tile position based on a passed in position.
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Config
{
    public const float tileSize = 1.28f;

    public const int chunkSize = 5;

    public const float gridOffsetX = 10;
    public const float gridOffsetY = 5;
    /// <summary>
    /// returns the coordinates to a tile based on a passed in world position. Taken from https://catlikecoding.com/unity/tutorials/hex-map/ but adapted to square grid.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public static Vector2 GetTileFromPosition(Vector2 position)
    {
        float x = position.x / tileSize;
        float y = position.y / tileSize;

        int iX = Mathf.RoundToInt(x);
        int iY = Mathf.RoundToInt(y);
        return new Vector2(iX, iY);
    }
}
