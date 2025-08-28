using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance;

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
}