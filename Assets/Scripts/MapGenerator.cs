using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public class Cell
    {
        public bool visited = false;
        // 0: Arriba, 1: Abajo, 2: Izquierda, 3: Derecha
        public bool[] status = new bool[4];
    }

    [SerializeField] public Vector2Int size;
    [SerializeField] public int initPosition = 0;
    [SerializeField] GameObject room;
    [SerializeField] public Vector2 roomSize;
    [SerializeField] public int nRooms;
    List<Cell> board;

    private void Start()
    {
        MazeGenerator();
    }

    public void MazeGenerator()
    {
        board = new List<Cell>();

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                board.Add(new Cell());
            }
        }

        int currentCell = initPosition;
        Stack<int> path = new Stack<int>();
        int num = 0;

        // Marcamos la primera como visitada antes del loop
        board[currentCell].visited = true;

        while (num < nRooms)
        {
            List<int> neighbours = CheckNeighbours(currentCell);

            if (neighbours.Count == 0)
            {
                if (path.Count == 0)
                    break;
                else
                    currentCell = path.Pop();
            }
            else
            {
                path.Push(currentCell);
                int newCell = neighbours[Random.Range(0, neighbours.Count)];

                // Lógica de apertura de paredes (status)
                if (newCell > currentCell)
                {
                    if (newCell - 1 == currentCell)
                    {
                        board[currentCell].status[3] = true; // Derecha
                        board[newCell].status[2] = true;    // Izquierda
                    }
                    else
                    {
                        board[currentCell].status[0] = true; // Arriba
                        board[newCell].status[1] = true;    // Abajo
                    }
                }
                else
                {
                    if (newCell + 1 == currentCell)
                    {
                        board[currentCell].status[2] = true;
                        board[newCell].status[3] = true;
                    }
                    else
                    {
                        board[currentCell].status[1] = true;
                        board[newCell].status[0] = true;
                    }
                }

                currentCell = newCell;
                board[currentCell].visited = true; // Marcamos la nueva celda
                num++;
            }
        }
        DungeonGenerator();
    }

    private void DungeonGenerator()
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                int index = i + j * size.x;

                // Solo instanciamos si la celda fue visitada
                if (board[index].visited)
                {
                    var newRoom = Instantiate(room, new Vector3(i * roomSize.x, 0, j * roomSize.y), Quaternion.identity, transform).GetComponent<Room>();
                    newRoom.UpdateRoom(board[index].status);
                }
            }
        }
    }

    private List<int> CheckNeighbours(int cellIndex)
    {
        List<int> neighbours = new List<int>();

        // Abajo (suponiendo que j-1 es index - size.x)
        if (cellIndex - size.x >= 0 && !board[cellIndex - size.x].visited)
        {
            neighbours.Add(cellIndex - size.x);
        }
        // Arriba
        if (cellIndex + size.x < board.Count && !board[cellIndex + size.x].visited)
        {
            neighbours.Add(cellIndex + size.x);
        }
        // Izquierda
        if (cellIndex % size.x != 0 && !board[cellIndex - 1].visited)
        {
            neighbours.Add(cellIndex - 1);
        }
        // Derecha
        if ((cellIndex + 1) % size.x != 0 && !board[cellIndex + 1].visited)
        {
            neighbours.Add(cellIndex + 1);
        }
        return neighbours;
    }
}