using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class Key
{
    public int number;
}

public class InventoryController : MonoBehaviour
{
    public static InventoryController Instance;

    public GameObject notEnoughRunesPanel;
    public GameObject changeKeyNumberPanel;
    public GameObject keyPanel;
    public TextMeshProUGUI neededKeyText;

    public TextMeshProUGUI key1Text;
    public TextMeshProUGUI key2Text;
    public TextMeshProUGUI currentKeyNumberText;
    
    public List<Key> keysCount;
    public int coinsCount;
    public int runesCount;

    public int neededKey = 1;

    public GameObject currentDoor;

    private int currentKey;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (key1Text.gameObject.activeSelf)
            key1Text.text = keysCount[0].number.ToString();
        if (key2Text.gameObject.activeSelf)
            key2Text.text = keysCount[1].number.ToString();
        
        if (currentKey == 1)
        {
            currentKeyNumberText.text = $"Current number: {keysCount[0].number}";
        }
        else if (currentKey == 2)
        {
            currentKeyNumberText.text = $"Current number: {keysCount[1].number}";
        }
    }

    public void SetNeededKeyDoor(int key, GameObject door)
    {
        neededKey = key;
        currentDoor = door;
    }

    public void OpenKeyPanel()
    {
        UIController.Instance.HideOpenKeyButton();
        neededKeyText.text = $"Need key: {neededKey}";
        currentKeyNumberText.text = $"Current number: {keysCount[0].number}";
        currentKeyNumberText.text = $"Current number: {keysCount[1].number}";
        keyPanel.SetActive(true);
    }
    
    public void HandleUseKey1()
    {
        if (keysCount[0].number != neededKey)
        {
            currentKey = 1;
            changeKeyNumberPanel.SetActive(true);
        }
        else
        {
            keyPanel.SetActive(false);
            currentDoor.SetActive(false);
        }
    }
    public void HandleUseKey2()
    {
        if (keysCount[1].number != neededKey)
        {
            currentKey = 2;
            changeKeyNumberPanel.SetActive(true);
        }
        else
        {
            keyPanel.SetActive(false);
            currentDoor.SetActive(false);
        }
    }

    public void HandleChangeKeyNumberMore()
    {
        if (runesCount <= 0)
        {
            notEnoughRunesPanel.SetActive(true);
            return;
        }

        runesCount--;
        
        if (currentKey == 1)
        {
            keysCount[0].number++;
        }
        else if (currentKey == 2)
        {
            keysCount[1].number++;
        }
    }
    public void HandleChangeKeyNumberLess()
    {
        if (runesCount <= 0)
        {
            notEnoughRunesPanel.SetActive(true);
            return;
        }
        
        if (currentKey == 1)
        {
            keysCount[0].number--;
            if (keysCount[0].number <= 0)
            {
                keysCount[0].number = 0;
            }
            else
            {
                runesCount--;
            }
        }
        else if (currentKey == 2)
        {
            keysCount[1].number--;
            if (keysCount[1].number <= 0)
            {
                keysCount[1].number = 0;
            }
            else
            {
                runesCount--;
            }
        }
    }
}
