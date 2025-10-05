// using System.Collections.Generic;
// using UnityEngine;
//
// public class CellMap : MonoBehaviour
// {
//     private Dictionary<int, Cell> cellDictionary;
//
//     private Vector3 _cellSize;
//     public Vector3 CellSize { get; set; }
//     
//     public Vector2 BoardSize { get; set; }
//
//     public void PutCell(Vector2Int cellPos, Cell cell)
//     {
//         cellDictionary[cellPos] = cell;
//     }
//
//     public Cell GetCellByPosInBoard(Vector2Int posInBoard)
//     {
//         return cellDictionary[posInBoard];
//     }
//
//     // public Cell GetCellByRealPos(Vector3 realPos)
//     // {
//     //     
//     // }
//     //
//     // private Vector2Int PosInBoardFromRealPos(Vector3 realPos)
//     // {
//     //     
//     // }
//     
//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {
//         cellDictionary = new Dictionary<Vector2Int, Cell>();
//     }
//
//     // Update is called once per frame
//     void Update()
//     {
//         
//     }
// }
