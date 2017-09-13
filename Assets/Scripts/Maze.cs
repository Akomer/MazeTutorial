using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PassageSelector), typeof(MazeRoomSettings))]
public class Maze : MonoBehaviour {

    public IntVector2 size;

    public MazeCell cellPrefab;
    public MazeWall wallPrefab;

    private MazeCell[,] cells;
    private PassageSelector passageSelector;
    private MazeRoomSettings mazeRoomSettings;
    private List<MazeRoom> rooms;

    private void Awake()
    {
        passageSelector = GetComponent<PassageSelector>();
        mazeRoomSettings = GetComponent<MazeRoomSettings>();
        rooms = new List<MazeRoom>();
    }

    public IEnumerator Generate()
    {
        yield return GenerateSmartRandomMaze();
    }

    public MazeCell GetCell(IntVector2 coordinates)
    {
        return cells[coordinates.x, coordinates.z];
    }

    private IEnumerator GenerateSmartRandomMaze()
    {
        var delay = new WaitForSeconds(0.0001f);
        InitCells();
        var activeCells = new List<MazeCell>();
        DoFirstGenerationStep(activeCells);
        while(activeCells.Count > 0)
        {
            yield return delay;
            DoNextGenerationStep(activeCells);
        }
        for (var i = 0; i < rooms.Count; i++)
        {
            rooms[i].Hide();
        }
    }

    private void DoFirstGenerationStep(List<MazeCell> activeCells)
    {
        var newCell = CreateCell(RandomCoordinates());
        newCell.Initialize(CreateRoom());
        activeCells.Add(newCell);
    }

    private void DoNextGenerationStep(List<MazeCell> activeCells)
    {
        var currentIndex = activeCells.Count - 1;
        var currentCell = activeCells[currentIndex];
        if (currentCell.IsFullyInitialized)
        {
            activeCells.RemoveAt(currentIndex);
            return;
        }
        var direction = currentCell.RandomUninitializedDirection();
        var coordinates = currentCell.coordinates + direction.ToIntVector2();
        if (MazeContainsCoordinates(coordinates))
        {
            var neighbor = GetCell(coordinates);
            if (neighbor == null)
            {
                neighbor = CreateCell(coordinates);
                CreatePassage(currentCell, neighbor, direction);
                activeCells.Add(neighbor);
            }
            else
            {
                CreatWall(currentCell, neighbor, direction);
            }
        }
        else
        {
            CreatWall(currentCell, null, direction);
        }

    }

    private void CreatePassage(MazeCell c1, MazeCell c2, MazeDirection direction)
    {
        var passagePrefab = passageSelector.PickRandomPassage();
        var passage = Instantiate<MazePassage>(passagePrefab);
        if (passage is MazeDoor)
        {
            c2.Initialize(CreateRoom());
        }
        else
        {
            c2.Initialize(c1.room);
        }
        passage.Initialize(c1, c2, direction);
        passage = Instantiate<MazePassage>(passagePrefab);
        passage.Initialize(c2, c1, direction.GetOpposite());
    }

    private void CreatWall(MazeCell c1, MazeCell c2, MazeDirection direction)
    {
        var wall = Instantiate<MazeWall>(wallPrefab);
        wall.Initialize(c1, c2, direction);
        if (c2 != null)
        {
            wall = Instantiate<MazeWall>(wallPrefab);
            wall.Initialize(c2, c1, direction.GetOpposite());
        }

    }

    private MazeRoom CreateRoom()
    {
        var newRoom = ScriptableObject.CreateInstance<MazeRoom>();
        newRoom.settings = mazeRoomSettings.NextSettings();
        rooms.Add(newRoom);
        return newRoom;
    }

    private IEnumerator GenerateRandomSteps()
    {
        var delay = new WaitForSeconds(0.01f);
        InitCells();
        var coord = RandomCoordinates();
        while(MazeContainsCoordinates(coord))
        {
            yield return delay;
            CreateCell(coord);
            while (GetCell(coord) != null)
            {
                coord += MazeDirections.RandomIntVector2Direction();
                if (!MazeContainsCoordinates(coord))
                    break;
            }
        }
    }

    private IEnumerator GenerateAllCell()
    {
        var delay = new WaitForSeconds(0.01f);
        InitCells();
        for (var x = 0; x < size.x; x++)
        {
            for (var z = 0; z < size.z; z++)
            {
                CreateCell(new IntVector2(x, z));
                yield return delay;
            }
        }
    }

    public IntVector2 RandomCoordinates()
    {
        return new IntVector2(UnityEngine.Random.Range(0, size.x), UnityEngine.Random.Range(0, size.z));
    }

    private bool MazeContainsCoordinates(IntVector2 coordinate)
    {
        return coordinate.x >= 0 && coordinate.x < size.x && coordinate.z >= 0 && coordinate.z < size.z;
    }

    private void InitCells()
    {
        cells = new MazeCell[size.x, size.z];
    }

    private MazeCell CreateCell(IntVector2 coord)
    {
        var newCell = Instantiate<MazeCell>(cellPrefab);
        cells[coord.x, coord.z] = newCell;
        newCell.name = "Maze cell " + coord.x + ", " + coord.z;
        newCell.coordinates = coord;
        newCell.transform.parent = transform;
        newCell.transform.localPosition = new Vector3(coord.x - size.x * 0.5f + 0.5f, 0f, coord.z - size.z * 0.5f + 0.5f);

        return newCell;
    }
}
