using TMPro;
using UnityEngine;

public class HubController : MonoBehaviour
{
   public static HubController Instance;

   public TextMeshProUGUI coinsText;
   
   public int coinsCount;
   
   private void Awake()
   {
      Instance = this;
   }

   private void Start()
   {
      coinsCount = PlayerPrefs.GetInt("coins", 0);
   }

   private void Update()
   {
      coinsText.text = coinsCount.ToString();
   }
}
