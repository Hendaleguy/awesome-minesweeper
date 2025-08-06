using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField]
    private int width;

    [SerializeField]
    private int height;

    [SerializeField]
    private int numMines;

    [SerializeField]
    private Cell[] cells;

    public Grid(int width, int height, int numMines)
    {
        this.width = width;
        this.height = height;
        this.numMines = numMines;
    }

    private void InitializeGrid()
    {
        cells = new Cell[width * height];
        
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
