using System;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public static InventoryController Instance;

    public int coinsCount;
    
    private void Awake()
    {
        Instance = this;
    }
}
