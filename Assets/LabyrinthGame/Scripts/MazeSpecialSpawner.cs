using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProceduralMaze
{
    public class MazeSpecialSpawner : MonoBehaviour
    {
        public static MazeSpecialSpawner Instance;

        public Dictionary<GameObject, MazeType> ballsInMaze;
        
        [Header("Special Prefabs")]
        [SerializeField] private GameObject[] specialPrefabs; // какие спец.объекты спавним
        [SerializeField] private int totalCount = 3;          // сколько всего заспавнить

        private bool spawned = false;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            ballsInMaze = new Dictionary<GameObject, MazeType>();
        }

        private void Start()
        {
            Invoke(nameof(SpawnSpecials), 1f);
        }

        [ContextMenu("Spawn Special Prefabs")]
        public void SpawnSpecials()
        {
            if (spawned) return;

            // Берём ВСЕ объекты в сцене, даже неактивные
            GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

            List<GameObject> mazeCells = new List<GameObject>();
            foreach (var obj in allObjects)
            {
                if (obj.name == "MazeCell(Clone)")
                {
                    mazeCells.Add(obj);
                }
            }

            if (mazeCells.Count == 0)
            {
                Debug.LogWarning("[MazeSpecialSpawner] Не найдено ни одного MazeCell(Clone)!");
                return;
            }

            int count = Mathf.Min(totalCount, specialPrefabs.Length, mazeCells.Count);

            for (int i = 0; i < count; i++)
            {
                int randomIndex = Random.Range(0, mazeCells.Count);
                GameObject targetCell = mazeCells[randomIndex];
                mazeCells.RemoveAt(randomIndex);

                // --- центр Floor внутри MazeCell ---
                Transform floor = targetCell.transform.Find("Floor");
                Vector3 pos;
                if (floor != null)
                {
                    Renderer rend = floor.GetComponent<Renderer>();
                    if (rend != null)
                    {
                        pos = rend.bounds.center; // ровно по центру XZ объекта Floor
                    }
                    else
                    {
                        pos = floor.position; // fallback: просто позиция Floor
                    }
                }
                else
                {
                    Debug.LogWarning($"[MazeSpecialSpawner] У {targetCell.name} нет объекта Floor!");
                    pos = targetCell.transform.position;
                }
                
                pos.y += 1f;

                GameObject ball = Instantiate(
                    specialPrefabs[i],
                    pos,
                    Quaternion.identity,
                    targetCell.transform
                );
                
                Transform mazeRoot = targetCell.transform.parent;
                float rotation = mazeRoot.localEulerAngles.y;

                switch (rotation)
                {
                    case 0f:
                        ballsInMaze.Add(ball, MazeType.m0f);
                        break;
                    case 90f:
                        ballsInMaze.Add(ball, MazeType.m90f);
                        break;
                    case 180f:
                        ballsInMaze.Add(ball, MazeType.m180f);
                        break;
                    case -90f:
                        ballsInMaze.Add(ball, MazeType.mN90f);
                        break;
                }
            }

            spawned = true;
        }

    }
}
