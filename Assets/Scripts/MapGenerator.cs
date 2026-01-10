using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public class Cell
    {
        public bool visited = false;
        // 0: Up, 1: Down, 2: Left, 3: Right
        public bool[] status = new bool[4];
    }

    [Header("Configuración")]
    public Vector2Int size;
    [SerializeField] private int initPosition = 0;
    [SerializeField] private int nRooms;
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private Vector2 roomOffset = new Vector2(10, 10);

    private List<Cell> board;

    // Setters públicos para que el Controller pueda usarlos
    public void SetNRooms(int value) => nRooms = value;
    public void SetInitPosition(int value) => initPosition = value;

    public void MazeGenerator()
    {
        board = new List<Cell>();

        // Llenar el tablero (Importante: j es Y, i es X)
        for (int j = 0; j < size.y; j++)
        {
            for (int i = 0; i < size.x; i++)
            {
                board.Add(new Cell());
            }
        }

        // Validar que la posición inicial no esté fuera de rango
        int currentCell = Mathf.Clamp(initPosition, 0, board.Count - 1);
        Stack<int> path = new Stack<int>();
        int roomsCreated = 0;

        while (roomsCreated < nRooms)
        {
            board[currentCell].visited = true;
            List<int> neighbours = CheckNeighbours(currentCell);

            if (neighbours.Count == 0)
            {
                if (path.Count == 0) break;
                currentCell = path.Pop();
            }
            else
            {
                path.Push(currentCell);
                int newCell = neighbours[Random.Range(0, neighbours.Count)];

                // Lógica de apertura de puertas
                if (newCell == currentCell + size.x) // Arriba
                {
                    board[currentCell].status[0] = true;
                    board[newCell].status[1] = true;
                }
                else if (newCell == currentCell - size.x) // Abajo
                {
                    board[currentCell].status[1] = true;
                    board[newCell].status[0] = true;
                }
                else if (newCell == currentCell - 1) // Izquierda
                {
                    board[currentCell].status[2] = true;
                    board[newCell].status[3] = true;
                }
                else if (newCell == currentCell + 1) // Derecha
                {
                    board[currentCell].status[3] = true;
                    board[newCell].status[2] = true;
                }

                currentCell = newCell;
                roomsCreated++;
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
                Cell currentData = board[i + j * size.x];
                if (currentData.visited) // Solo instanciar si fue parte del camino
                {
                    Vector3 pos = new Vector3(i * roomOffset.x, 0, j * roomOffset.y);
                    GameObject go = Instantiate(roomPrefab, pos, Quaternion.identity, transform);
                    go.GetComponent<Room>().UpdateRoom(currentData.status);
                }
            }
        }
    }

    private List<int> CheckNeighbours(int cell)
    {
        List<int> neighbours = new List<int>();

        // ARRIBA (North)
        if (cell + size.x < board.Count && !board[cell + size.x].visited)
            neighbours.Add(cell + size.x);

        // ABAJO (South)
        if (cell - size.x >= 0 && !board[cell - size.x].visited)
            neighbours.Add(cell - size.x);

        // IZQUIERDA (West)
        if (cell % size.x != 0 && !board[cell - 1].visited)
            neighbours.Add(cell - 1);

        // DERECHA (East)
        if ((cell + 1) % size.x != 0 && !board[cell + 1].visited)
            neighbours.Add(cell + 1);

        return neighbours;
    }
}