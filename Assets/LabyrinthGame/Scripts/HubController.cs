using System;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class HubController : MonoBehaviour
{
    public static HubController Instance;

    [Header("Buttons")]
    public RectTransform swimButton;
    public RectTransform leaveButton;

    public GameObject player;
    public Animator animator;
    
    public Transform swimPos;
    public Transform exitPos;
    
    [Header("UI")]
    public TextMeshProUGUI coinsText;
    public int coinsCount;

    [Header("Animation Settings")]
    public float animDuration = 0.5f; // время анимации
    public float offsetY = 500f;      // насколько уходит вниз под экран

    private Vector2 swimOriginalPos;
    private Vector2 leaveOriginalPos;

    [Header("Purchases")]
    public GameObject location1;
    public GameObject location2;
    public GameObject location3;

    public TextMeshProUGUI location1ButtonText;
    public TextMeshProUGUI location2ButtonText;
    public TextMeshProUGUI location3ButtonText;

    public GameObject notEnoughMoneyPanel;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        coinsCount = PlayerPrefs.GetInt("coins", 0);

        swimOriginalPos = swimButton.anchoredPosition;
        leaveOriginalPos = leaveButton.anchoredPosition;

        swimButton.anchoredPosition = swimOriginalPos - new Vector2(0, offsetY);
        leaveButton.anchoredPosition = leaveOriginalPos - new Vector2(0, offsetY);

        // Загружаем покупки
        if (PlayerPrefs.GetInt("location1", 0) == 1)
        {
            location1.SetActive(true);
            location1ButtonText.text = "PURCHASED";
        }

        if (PlayerPrefs.GetInt("location2", 0) == 1)
        {
            location2.SetActive(true);
            location2ButtonText.text = "PURCHASED";
        }

        if (PlayerPrefs.GetInt("location3", 0) == 1)
        {
            location3.SetActive(true);
            location3ButtonText.text = "PURCHASED";
        }
    }

    private void Update()
    {
        coinsText.text = coinsCount.ToString();
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.CompareTag("Water"))
        {
            ShowSwimButton();
        }
    }
    private void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.CompareTag("Water"))
        {
            HideSwimButton();
        }
    }

    public void Swim()
    {
        HideSwimButton();
        ShowLeaveButton();
        player.transform.position = swimPos.position;
        animator.SetBool("Swim", true);
        player.GetComponent<PlayerController>().moveSpeed = 2;
    }

    public void Leave()
    {
        HideLeaveButton();
        player.transform.position = exitPos.position;
        player.GetComponent<PlayerController>().moveSpeed = 5;
        animator.SetBool("Swim", false);
    }

    public void HandlePlay()
    {
        SceneManager.LoadScene(1);
    }
    
    public void ShowButton(RectTransform button, Vector2 originalPos)
    {
        button.DOAnchorPos(originalPos, animDuration).SetEase(Ease.OutBack);
    }
    
    public void HideButton(RectTransform button, Vector2 originalPos)
    {
        button.DOAnchorPos(originalPos - new Vector2(0, offsetY), animDuration).SetEase(Ease.InBack);
    }

    public void ShowSwimButton() => ShowButton(swimButton, swimOriginalPos);
    public void HideSwimButton() => HideButton(swimButton, swimOriginalPos);

    public void ShowLeaveButton() => ShowButton(leaveButton, leaveOriginalPos);
    public void HideLeaveButton() => HideButton(leaveButton, leaveOriginalPos);


    // --- Методы покупки ---
    public void BuyLocation1()
    {
        if (coinsCount >= 50)
        {
            coinsCount -= 50;
            PlayerPrefs.SetInt("coins", coinsCount);
            location1.SetActive(true);
            location1ButtonText.text = "PURCHASED";
            PlayerPrefs.SetInt("location1", 1);
            PlayerPrefs.Save();
        }
        else
        {
            notEnoughMoneyPanel.SetActive(true);
        }
    }

    public void BuyLocation2()
    {
        if (coinsCount >= 50)
        {
            coinsCount -= 50;
            PlayerPrefs.SetInt("coins", coinsCount);
            location2.SetActive(true);
            location2ButtonText.text = "PURCHASED";
            PlayerPrefs.SetInt("location2", 1);
            PlayerPrefs.Save();
        }
        else
        {
            notEnoughMoneyPanel.SetActive(true);
        }
    }

    public void BuyLocation3()
    {
        if (coinsCount >= 50)
        {
            coinsCount -= 50;
            PlayerPrefs.SetInt("coins", coinsCount);
            location3.SetActive(true);
            location3ButtonText.text = "PURCHASED";
            PlayerPrefs.SetInt("location3", 1);
            PlayerPrefs.Save();
        }
        else
        {
            notEnoughMoneyPanel.SetActive(true);
        }
    }
}
