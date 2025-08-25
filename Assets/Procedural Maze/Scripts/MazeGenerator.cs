using UnityEngine;

namespace ProceduralMaze
{
    public class MazeGenerator : MonoBehaviour
    {
        public static MazeGenerator Instance;

        [SerializeField] MazeController mazeController;
        [SerializeField] GameObject mazeCellPrefab;

        public float rotation;
        
        public GameObject prefab;
        
        public GameObject spawnPos;

        public GameObject entryObject;
        
        private float cellSize;

        private void Awake()
        {
            Instance = this;
            CalculateCellSize();
        }

        private void Start()
        {
            GenerateMaze();
        }

        void CalculateCellSize()
        {
            if (mazeCellPrefab == null)
            {
                Debug.LogError("Maze cell prefab is not assigned!");
                return;
            }

            Renderer rend = mazeCellPrefab.GetComponentInChildren<Renderer>();
            if (rend != null)
            {
                cellSize = rend.bounds.size.x;
                Debug.Log($"[MazeGenerator] Авто cellSize = {cellSize}");
            }
            else
            {
                Debug.LogWarning("Maze cell prefab has no Renderer! Использую 1.0f по умолчанию.");
                cellSize = 1f;
            }
        }

        [ContextMenu("GENERATE MAZE")]
        public void GenerateMaze()
        {
            MazeCell[,] maze = mazeController.GetMaze();
            GenerateMaze(maze);
        }

        private void GenerateMaze(MazeCell[,] maze)
        {
            entryObject = null;

            for (int x = 0; x < mazeController.mazeWidth; x++)
            {
                for (int y = 0; y < mazeController.mazeHeight; y++)
                {
                    GameObject newCell = Instantiate(
                        mazeCellPrefab,
                        new Vector3(x * cellSize, 0f, y * cellSize),
                        Quaternion.identity,
                        transform
                    );

                    MazeCellObject mazeCell = newCell.GetComponent<MazeCellObject>();

                    bool top = maze[x, y].topWall;
                    bool left = maze[x, y].leftWall;
                    bool right = maze[x, y].rightWall;
                    bool bottom = maze[x, y].bottomWall;

                    mazeCell.Init(top, bottom, left, right);

                    // Если это входная клетка — сохраняем ссылку на объект
                    if (mazeController.entryPosition.x == x && mazeController.entryPosition.y == y)
                    {
                        entryObject = newCell;
                        Debug.Log($"[MazeGenerator] EntryObject установлен: {newCell.name}");
                    }
                }
            }
            
            GameObject mazeParent = Instantiate(prefab);
            mazeParent.transform.position = entryObject.transform.position;
            transform.SetParent(mazeParent.transform);
            mazeParent.transform.localRotation = Quaternion.Euler(0f, rotation, 0f);
            mazeParent.transform.position = spawnPos.transform.position;

            switch (rotation)
            {
                case 0:
                    LevelController.Instance._maze0f = gameObject;
                    break;
                case 90f:
                    LevelController.Instance._maze90f = gameObject;
                    break;
                case 180f:
                    LevelController.Instance._maze180f = gameObject;
                    break;
                case -90f:
                    LevelController.Instance._mazeM90f = gameObject;
                    break;
            }
            
            gameObject.SetActive(false);
        }
    }
}
