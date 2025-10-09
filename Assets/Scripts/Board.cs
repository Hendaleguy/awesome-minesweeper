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
    
    private Dictionary<int, Cell> cellIDDictionary;
    private Dictionary<Vector2Int, Cell> cellPosDictionary;
    private Cell currentCell;

    private void InitializeGrid()
    {
        cellIDDictionary = new Dictionary<int, Cell>();
        cellPosDictionary = new Dictionary<Vector2Int, Cell>();
        
        Vector3 cellSize = cellObject.GetComponent<MeshRenderer>().bounds.size;
        Debug.Log(cellSize);
        for (int i = 0; i < width * height; i++)
        {
            GameObject newCellObject = Instantiate(cellObject, transform);
            int xPos = i % width;
            int zPos = i / width;
            newCellObject.transform.Translate(xPos * cellSize.x, 0, zPos * cellSize.z, Space.World);
            
            Cell cell = newCellObject.GetComponent<Cell>();
            Vector2Int cellPos = new Vector2Int(xPos, zPos);
            cell.PosInBoard = cellPos;
            
            cellIDDictionary[newCellObject.GetInstanceID()] = cell;
            cellPosDictionary[cellPos] = cell;
        }
    }

    private Cell GetCellFromRaycastHit(RaycastHit hit)
    {
        int cellID = hit.collider.gameObject.GetInstanceID();
        Cell targetCell = cellIDDictionary[cellID];

        return targetCell;
    }

    public void HandleClick(RaycastHit hit)
    {
        Cell targetCell = GetCellFromRaycastHit(hit);

        if (currentCell)
        {
            currentCell.UnClick();
        }

        currentCell = targetCell;
        currentCell.Click();
    }

    /**
     * Actual click activation logic happens on release
     */
    public void HandleClickRelease()
    {
        if (currentCell)
        {
            currentCell.ClickRelease();
        }
    }
    
    public void HandleRightClick(RaycastHit hit)
    {
        Cell targetCell = GetCellFromRaycastHit(hit);

        targetCell.HandleFlag();
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
