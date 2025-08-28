using System;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    public RectTransform openKeyPanelButton;
    
    [Header("UI Elements")]
    public RectTransform keysHandler;
    public RectTransform coinsHandler;

    public TextMeshProUGUI runesText;
    public TextMeshProUGUI _keysText;
    public TextMeshProUGUI _coinsText;
    public TextMeshProUGUI _ballsText;

    [Header("Animation Settings")]
    public float duration = 0.5f; // время анимации
    public float offset = 1000f;  // смещение за экран

    private bool showingKeys = true; // какой UI сейчас активен
    private bool isAnimating = false; // флаг анимации

    private Vector3 keysPos;
    private Vector3 coinsPos;

    private Vector2 openKeyButtonPos; // стартовая позиция кнопки

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        keysPos = keysHandler.anchoredPosition;
        coinsPos = coinsHandler.anchoredPosition;

        keysHandler.gameObject.SetActive(true);
        coinsHandler.gameObject.SetActive(false);
        
        HandleCoinText();

        // сохраняем изначальную позицию кнопки и уводим вниз
        openKeyButtonPos = openKeyPanelButton.anchoredPosition;
        openKeyPanelButton.anchoredPosition = openKeyButtonPos - new Vector2(0, offset);
    }

    private void Update()
    {
        _keysText.text = InventoryController.Instance.keysInInventory.ToString();
        runesText.text = InventoryController.Instance.runesCount.ToString();
    }

    public void HandleLeave()
    {
        SceneManager.LoadScene("HubScene");
    }
    
    public void HandleCoinText()
    {
        _coinsText.text = InventoryController.Instance.coinsCount.ToString();
    }
    public void HandleBallText()
    {
        _ballsText.text = InventoryController.Instance.coinsCount.ToString();
    }

    public void ToggleUI()
    {
        if (isAnimating) return;

        keysHandler.DOKill();
        coinsHandler.DOKill();
        isAnimating = true;

        if (showingKeys)
        {
            keysHandler.DOAnchorPosX(keysPos.x - offset, duration).OnComplete(() =>
            {
                keysHandler.gameObject.SetActive(false);
                keysHandler.anchoredPosition = keysPos;

                coinsHandler.anchoredPosition = new Vector2(coinsPos.x - offset, coinsPos.y);
                coinsHandler.gameObject.SetActive(true);
                coinsHandler.DOAnchorPosX(coinsPos.x, duration).OnComplete(() =>
                {
                    isAnimating = false;
                });
            });
        }
        else
        {
            coinsHandler.DOAnchorPosX(coinsPos.x - offset, duration).OnComplete(() =>
            {
                coinsHandler.gameObject.SetActive(false);
                coinsHandler.anchoredPosition = coinsPos;

                keysHandler.anchoredPosition = new Vector2(keysPos.x - offset, keysPos.y);
                keysHandler.gameObject.SetActive(true);
                keysHandler.DOAnchorPosX(keysPos.x, duration).OnComplete(() =>
                {
                    isAnimating = false;
                });
            });
        }

        showingKeys = !showingKeys;
    }

    public void ShowOpenKeyButton()
    {
        openKeyPanelButton.DOKill();
        openKeyPanelButton.DOAnchorPos(openKeyButtonPos, duration).SetEase(Ease.OutBack);
    }

    public void HideOpenKeyButton()
    {
        openKeyPanelButton.DOKill();
        openKeyPanelButton.DOAnchorPos(openKeyButtonPos - new Vector2(0, offset), duration).SetEase(Ease.InBack);
    }
}
