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
      // Выключаем все
      _maze0f.SetActive(false);
      _maze90f.SetActive(false);
      _maze180f.SetActive(false);
      _mazeM90f.SetActive(false);
      
      switch (maze)
      {
         case MazeType.m0f:
            _maze0f.SetActive(true);
            break;
         case MazeType.m90f:
            _maze90f.SetActive(true);
            break;
         case MazeType.m180f:
            _maze180f.SetActive(true);
            break;
         case MazeType.mN90f:
            _mazeM90f.SetActive(true);
            break;
      }
   }
}
