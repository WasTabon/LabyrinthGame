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
}
