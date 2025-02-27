using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

public class GridManager : MonoBehaviour
{
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

    [Button]
    public void UpdateGrid()
    {
        var totalCellsNeeded = gridSize * gridSize;
        var orthoHeight = cameraManager.OrtoHeight;
        var orthoWidth = cameraManager.OrtoWidth;
        var cellSize = Mathf.Min(orthoWidth / gridSize, orthoHeight / gridSize);
        var startX = -orthoWidth / 2 + cellSize / 2;
        var startY = orthoHeight / 2 - cellSize / 2;

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
                var cellPosition = new Vector3(startX + x * cellSize, startY - y * cellSize, 0);
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
    }
}