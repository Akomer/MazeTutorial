using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MazeDirections
{
    private static int _count = -1;
    public static int Count
    {
        get
        {
            if (_count == -1)
                _count = System.Enum.GetValues(typeof(MazeDirection)).Length;
            return _count;
        }
    }

    private static readonly IntVector2[] vectors = {
        new IntVector2(0, 1),
        new IntVector2(1, 0),
        new IntVector2(0, -1),
        new IntVector2(-1, 0)
    };

    public static IntVector2 ToIntVector2(this MazeDirection direction)
    {
        return vectors[(int)direction];
    }

    private static MazeDirection[] opposites = {
        MazeDirection.South,
        MazeDirection.West,
        MazeDirection.North,
        MazeDirection.East
    };

    public static MazeDirection GetOpposite(this MazeDirection direction)
    {
        return opposites[(int)direction];
    }
   
    public static MazeDirection RandomValue
    {
        get
        {
            return (MazeDirection)Random.Range(0, Count);
        }
    }

    public static MazeDirection GetNextClockwise(this MazeDirection direction)
    {
        return (MazeDirection)(((int)direction + 1) % Count);
    }

    public static MazeDirection GetNextCounterclockwise(this MazeDirection direction)
    {
        return (MazeDirection)(((int)direction + Count - 1) % Count);
    }

    public static IntVector2 RandomIntVector2Direction()
    {
        return vectors[Random.Range(0, Count)];
    }

    private static Quaternion[] rotations = {
        Quaternion.identity,
        Quaternion.Euler(0f, 90f, 0f),
        Quaternion.Euler(0f, 180f, 0f),
        Quaternion.Euler(0f, 270f, 0f)
    };

    public static Quaternion ToRotation(this MazeDirection direction)
    {
        return rotations[(int)direction];
    }

}
