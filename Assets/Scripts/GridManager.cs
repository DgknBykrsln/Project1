using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

public class GridManager : MonoBehaviour
{
    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    private static readonly Direction[] directions = { Direction.Up, Direction.Down, Direction.Left, Direction.Right };

    [SerializeField, BoxGroup("Settings")] private int gridSize;
    [SerializeField, ReadOnly] private List<Cell> cells = new();

    private CameraManager cameraManager;
    private ObjectPooler objectPooler;

    [Inject]
    private void Construct(CameraManager _cameraManager, ObjectPooler _objectPooler)
    {
        cameraManager = _cameraManager;
        objectPooler = _objectPooler;
        UpdateGrid();
    }

    private (int[,], Dictionary<(int, int), Cell>) CreateGridState()
    {
        var gridStates = new int[gridSize, gridSize];
        var cellMap = new Dictionary<(int, int), Cell>();

        for (var i = 0; i < cells.Count; i++)
        {
            var x = i % gridSize;
            var y = i / gridSize;
            cellMap[(x, y)] = cells[i];
            gridStates[x, y] = cells[i].State == Cell.CellState.Active ? 1 : 0;
        }

        return (gridStates, cellMap);
    }

    private HashSet<Cell> FindConnectedCells(int startX, int startY, int[,] gridStates, Dictionary<(int, int), Cell> cellMap, HashSet<(int, int)> visited)
    {
        var group = new HashSet<Cell>();
        var queue = new Queue<(int, int)>();
        queue.Enqueue((startX, startY));
        visited.Add((startX, startY));
        group.Add(cellMap[(startX, startY)]);

        while (queue.Count > 0)
        {
            var (x, y) = queue.Dequeue();

            foreach (var dir in directions)
            {
                var (newX, newY) = GetNewCoordinates(x, y, dir);

                if (IsValidCell(newX, newY, gridStates, visited))
                {
                    visited.Add((newX, newY));
                    queue.Enqueue((newX, newY));
                    group.Add(cellMap[(newX, newY)]);
                }
            }
        }

        return group;
    }

    private static (int, int) GetNewCoordinates(int x, int y, Direction dir)
    {
        return dir switch
        {
            Direction.Up => (x, y + 1),
            Direction.Down => (x, y - 1),
            Direction.Left => (x - 1, y),
            Direction.Right => (x + 1, y),
            _ => (x, y)
        };
    }

    private bool IsValidCell(int x, int y, int[,] gridStates, HashSet<(int, int)> visited)
    {
        return x >= 0 && x < gridSize && y >= 0 && y < gridSize && gridStates[x, y] == 1 && !visited.Contains((x, y));
    }

    [Button]
    public void UpdateGrid()
    {
        var totalCellsNeeded = gridSize * gridSize;
        var orthoHeight = cameraManager.Height;
        var orthoWidth = cameraManager.Width;
        var cellSize = Mathf.Min(orthoWidth / gridSize, orthoHeight / gridSize);
        var startX = -orthoWidth / 2 + cellSize / 2;

        var gridTopY = (gridSize / 2f) * cellSize;
        var pivotOffset = cellSize / 2f;

        transform.position = new Vector3(0, gridTopY + pivotOffset, 0);

        for (var i = 0; i < totalCellsNeeded || i < cells.Count; i++)
        {
            if (i < totalCellsNeeded)
            {
                Cell cell;
                if (i < cells.Count)
                {
                    cell = cells[i];
                }
                else
                {
                    cell = objectPooler.GetObjectFromPool<Cell>();
                    cells.Add(cell);
                }

                var x = i % gridSize;
                var y = i / gridSize;
                var cellPosition = new Vector3(startX + x * cellSize, gridTopY - y * cellSize, 0);
                var targetCellScale = Vector3.one * cellSize / cell.BoundSize;
                var cellName = $"Cell ({x},{y})";

                cell.PrepareCell(transform, cellPosition, targetCellScale, cellName);
            }
            else
            {
                objectPooler.SendObjectToPool(cells[i]);
                cells.RemoveAt(i);
                i--;
            }
        }

        transform.position = cameraManager.GetScreenTopPosition();
    }


    public void NotifyStateChange()
    {
        var (gridStates, cellMap) = CreateGridState();
        var cellsToSwitch = new HashSet<Cell>();
        var visited = new HashSet<(int, int)>();

        for (var x = 0; x < gridSize; x++)
        {
            for (var y = 0; y < gridSize; y++)
            {
                if (gridStates[x, y] == 1 && !visited.Contains((x, y)))
                {
                    var group = FindConnectedCells(x, y, gridStates, cellMap, visited);
                    if (group.Count >= 3)
                    {
                        cellsToSwitch.UnionWith(group);
                    }
                }
            }
        }

        foreach (var cell in cellsToSwitch)
        {
            cell.ToggleState();
        }
    }
}