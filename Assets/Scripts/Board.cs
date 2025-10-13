using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

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
    private HashSet<Vector2Int> minePosSet;
     
    private Cell currentCell;

    private void InitializeGrid()
    {
        InstantiateCells();
        DistributeMines();
        AssignNeighborNumbers();
    }

    private void InstantiateCells()
    {
        cellIDDictionary = new Dictionary<int, Cell>();
        cellPosDictionary = new Dictionary<Vector2Int, Cell>();
        
        Vector3 cellSize = cellObject.GetComponent<MeshRenderer>().bounds.size;
        
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
        
        StaticBatchingUtility.Combine(gameObject);
    }
    
    private void DistributeMines()
    {
        minePosSet = new HashSet<Vector2Int>();
        
        List<int> mineIndexes = GenerateMineIndexes();

        foreach (Vector2Int minePos in mineIndexes.Select(IndexToPos))
        {
            minePosSet.Add(minePos);
            cellPosDictionary[minePos].AddMine();
        }
    }

    private void AssignNeighborNumbers()
    {
        foreach (List<Vector2Int> mineNeighbors in minePosSet.Select(GetNeighborsOfPos))
        {
            foreach (Vector2Int neighborCell in mineNeighbors)
            {
                cellPosDictionary[neighborCell].NumMineNeighbors += 1;
            }
        }
    }

    private List<Vector2Int> GetNeighborsOfPos(Vector2Int pos)
    {

        List<Vector2Int> transforms = new List<Vector2Int>()
        {
            new Vector2Int(0, 1),
            new Vector2Int(1, 1),
            new Vector2Int(1, 0),
            new Vector2Int(1, -1),
            new Vector2Int(0, -1),
            new Vector2Int(-1, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(-1, 1),
        };

        return transforms.Select(t => pos + t).Where(adj => cellPosDictionary.ContainsKey(adj)).ToList();
    }

    private Vector2Int IndexToPos(int index)
    {
        return new Vector2Int(index % width, index / width);
    }

    private int PosToIndex(Vector2Int pos)
    {
        return pos.y * width + pos.x;
    }

    

    private List<int> GenerateMineIndexes()
    {
        Random random = new Random();
        
        List<int> mineIndexes = new List<int>();
        for (int i = 0; i < numMines; i++)
        {
            mineIndexes.Add(random.Next(width * height));
        }

        return mineIndexes;
    }

    private Cell GetCellFromRaycastHit(RaycastHit hit)
    {
        int cellID = hit.collider.gameObject.GetInstanceID();
        Cell targetCell = cellIDDictionary[cellID];

        return targetCell;
    }

    /**
     * Routes click to correct cell
     */
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
        if (!currentCell)
            return;
        
        currentCell.UnClick();
        PropagateReveal(currentCell.PosInBoard);
    }

    private void PropagateReveal(Vector2Int cellPos)
    {
        Queue<Vector2Int> cellPosQueue = new Queue<Vector2Int>();
        cellPosQueue.Enqueue(cellPos);

        while (cellPosQueue.TryDequeue(out Vector2Int result))
        {
            Cell cell = cellPosDictionary[result];
            
            cell.Reveal();

            // TODO: double check that losing mechanism is taken into account here
            if (cell.NumMineNeighbors != 0)
                continue;

            IEnumerable<Vector2Int> eligibleNeighbors = GetNeighborsOfPos(result);
            foreach (Vector2Int neighborPos in eligibleNeighbors)
            {
                if (!cellPosDictionary[neighborPos].IsRevealed && !cellPosQueue.Contains(neighborPos))
                    cellPosQueue.Enqueue(neighborPos);
            }
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
}
