using ProceduralMaze;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance;

    public AudioClip looseSound;
    public AudioClip winSound;
    
    public GameObject loosePanel;
    public GameObject winPanel;
    
    public GameObject _maze0f;
    public GameObject _maze90f;
    public GameObject _maze180f;
    public GameObject _mazeM90f;

    private void Awake()
    {
        Instance = this;
    }

    public void ActivateMaze(MazeType maze)
    {
        if (_maze0f != null) _maze0f.SetActive(false);
        if (_maze90f != null) _maze90f.SetActive(false);
        if (_maze180f != null) _maze180f.SetActive(false);
        if (_mazeM90f != null) _mazeM90f.SetActive(false);
        
        switch (maze)
        {
            case MazeType.m0f:
                if (_maze0f != null) _maze0f.SetActive(true);
                break;
            case MazeType.m90f:
                if (_maze90f != null) _maze90f.SetActive(true);
                break;
            case MazeType.m180f:
                if (_maze180f != null) _maze180f.SetActive(true);
                break;
            case MazeType.mN90f:
                if (_mazeM90f != null) _mazeM90f.SetActive(true);
                break;
        }
    }

    public void CheckWinCondition()
    {
        if (InventoryController.Instance.ballsCount >= 3)
        {
            MusicController.Instance.PlaySpecificSound(winSound);
            winPanel.SetActive(true);
            Debug.Log("Win");
        }
    }
    
    public void CheckLoseCondition()
    {
        // Если у игрока нет ключей и меньше 3 ball
        if (InventoryController.Instance.keysInInventory <= 0 &&
            InventoryController.Instance.ballsCount < 3)
        {
            // проверяем, есть ли ещё ball'ы в закрытых лабиринтах
            foreach (var kvp in MazeSpecialSpawner.Instance.ballsInMaze)
            {
                foreach (DoorController door in InventoryController.Instance.doors)
                {
                    if (kvp.Value == door.door.MazeType)
                    {
                        if (door.door.isOpened == false)
                        {
                            MusicController.Instance.PlaySpecificSound(looseSound);
                            loosePanel.SetActive(false);
                            Debug.Log("Loose");
                        }
                    }
                }
            }
        }
    }

}