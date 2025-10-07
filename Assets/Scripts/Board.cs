using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField]
    private int width;

    [SerializeField]
    private int height;

    [SerializeField]
    private int numMines;

    [SerializeField]
    private GameObject cellObject;
    
    private Dictionary<int, Cell> cellDictionary;
    private Cell currentCell;
    private Cell[] cells;

    private void InitializeGrid()
    {
        cellDictionary = new Dictionary<int, Cell>();
        Vector3 cellSize = cellObject.GetComponent<MeshRenderer>().bounds.size;
        Debug.Log(cellSize);
        for (int i = 0; i < width * height; i++)
        {
            GameObject newCellObject = Instantiate(cellObject, transform);
            int xPos = i % width;
            int zPos = i / width;
            newCellObject.transform.Translate(xPos * cellSize.x, 0, zPos * cellSize.z, Space.World);
            Cell cell = newCellObject.GetComponent<Cell>();
            cell.PosInBoard = new Vector2Int(xPos, zPos);
            cellDictionary[newCellObject.GetInstanceID()] = cell;
        }
    }

    public void HandleClick(RaycastHit hit)
    {
        int cellID = hit.collider.gameObject.GetInstanceID();

        Cell targetCell = cellDictionary[cellID];

        if (currentCell)
        {
            currentCell.UnClick();
        }

        currentCell = targetCell;
        currentCell.Click();
    }

    public void HandleClickRelease()
    {
        if (currentCell)
        {
            currentCell.ClickRelease();
        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        InitializeGrid();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
