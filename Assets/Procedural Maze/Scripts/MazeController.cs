using System.Collections.Generic;
using UnityEngine;

namespace ProceduralMaze
{
    public class MazeController : MonoBehaviour
    {
        [Range(5, 100)] public int mazeWidth = 5, mazeHeight = 5;
        public bool generateEntry = true;

        private MazeCell[,] maze;

        // теперь храним и координату, и саму клетку
        public Vector2Int entryPosition;
        public MazeCell entryCell;

        public MazeCell[,] GetMaze()
        {
            maze = new MazeCell[mazeWidth, mazeHeight];

            for (int x = 0; x < mazeWidth; x++)
            {
                for (int y = 0; y < mazeHeight; y++)
                {
                    maze[x, y] = new MazeCell(x, y);
                }
            }

            if (generateEntry)
                SetEntry();

            CarvePath(entryPosition.x, entryPosition.y);

            return maze;
        }

        void CarvePath(int x, int y)
        {
            Stack<Vector2Int> path = new Stack<Vector2Int>();
            path.Push(new Vector2Int(x, y));
            maze[x, y].visited = true;

            while (path.Count > 0)
            {
                Vector2Int current = path.Peek();
                List<Vector2Int> neighbors = GetValidNeighbors(current);

                if (neighbors.Count == 0)
                {
                    path.Pop();
                }
                else
                {
                    Vector2Int next = neighbors[UnityEngine.Random.Range(0, neighbors.Count)];
                    BreakWalls(current, next);
                    maze[next.x, next.y].visited = true;
                    path.Push(next);
                }
            }
        }

        List<Vector2Int> GetValidNeighbors(Vector2Int cell)
        {
            List<Vector2Int> neighbors = new List<Vector2Int>();

            if (cell.x > 0 && !maze[cell.x - 1, cell.y].visited)
                neighbors.Add(new Vector2Int(cell.x - 1, cell.y));

            if (cell.x < mazeWidth - 1 && !maze[cell.x + 1, cell.y].visited)
                neighbors.Add(new Vector2Int(cell.x + 1, cell.y));

            if (cell.y > 0 && !maze[cell.x, cell.y - 1].visited)
                neighbors.Add(new Vector2Int(cell.x, cell.y - 1));

            if (cell.y < mazeHeight - 1 && !maze[cell.x, cell.y + 1].visited)
                neighbors.Add(new Vector2Int(cell.x, cell.y + 1));

            return neighbors;
        }

        void BreakWalls(Vector2Int first, Vector2Int second)
        {
            if (first.x < second.x)
            {
                maze[first.x, first.y].rightWall = false;
                maze[second.x, second.y].leftWall = false;
            }
            else if (first.x > second.x)
            {
                maze[first.x, first.y].leftWall = false;
                maze[second.x, second.y].rightWall = false;
            }
            else if (first.y < second.y)
            {
                maze[first.x, first.y].topWall = false;
                maze[second.x, second.y].bottomWall = false;
            }
            else if (first.y > second.y)
            {
                maze[first.x, first.y].bottomWall = false;
                maze[second.x, second.y].topWall = false;
            }
        }

        void SetEntry()
        {
            int entryY = UnityEngine.Random.Range(0, mazeHeight);
            entryPosition = new Vector2Int(0, entryY);

            entryCell = maze[entryPosition.x, entryPosition.y];
            entryCell.leftWall = false; // убираем только вход

            Debug.Log($"Entry at {entryPosition} -> Cell ref set");
        }
    }

    public class MazeCell
    {
        public bool visited;
        public int x, y;
        public bool topWall = true, bottomWall = true, leftWall = true, rightWall = true;

        public MazeCell(int x, int y)
        {
            this.x = x;
            this.y = y;
            visited = false;
        }
    }
}
