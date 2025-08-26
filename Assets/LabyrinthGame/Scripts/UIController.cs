using UnityEngine;
using DG.Tweening;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    
    [Header("UI Elements")]
    public RectTransform keysHandler;
    public RectTransform coinsHandler;

    public TextMeshProUGUI _keysText;
    public TextMeshProUGUI _coinsText;

    [Header("Animation Settings")]
    public float duration = 0.5f; // время анимации
    public float offset = 1000f; // смещение за экран

    private bool showingKeys = true; // какой UI сейчас активен
    private bool isAnimating = false; // флаг анимации

    private Vector3 keysPos;
    private Vector3 coinsPos;

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
    }

    public void HandleCoinText()
    {
        _coinsText.text = InventoryController.Instance.coinsCount.ToString();
    }

    public void ToggleUI()
    {
        if (isAnimating) return; // если анимация идёт, выходим

        keysHandler.DOKill();
        coinsHandler.DOKill();
        isAnimating = true;

        if (showingKeys)
        {
            // keysHandler улетает влево
            keysHandler.DOAnchorPosX(keysPos.x - offset, duration).OnComplete(() =>
            {
                keysHandler.gameObject.SetActive(false);
                keysHandler.anchoredPosition = keysPos; // возвращаем на стартовую позицию

                // coinsHandler появляется слева и летит на своё место
                coinsHandler.anchoredPosition = new Vector2(coinsPos.x - offset, coinsPos.y);
                coinsHandler.gameObject.SetActive(true);
                coinsHandler.DOAnchorPosX(coinsPos.x, duration).OnComplete(() =>
                {
                    isAnimating = false; // анимация завершена
                });
            });
        }
        else
        {
            // coinsHandler улетает влево
            coinsHandler.DOAnchorPosX(coinsPos.x - offset, duration).OnComplete(() =>
            {
                coinsHandler.gameObject.SetActive(false);
                coinsHandler.anchoredPosition = coinsPos;

                // keysHandler появляется слева и летит на своё место
                keysHandler.anchoredPosition = new Vector2(keysPos.x - offset, keysPos.y);
                keysHandler.gameObject.SetActive(true);
                keysHandler.DOAnchorPosX(keysPos.x, duration).OnComplete(() =>
                {
                    isAnimating = false; // анимация завершена
                });
            });
        }

        showingKeys = !showingKeys;
    }
}
