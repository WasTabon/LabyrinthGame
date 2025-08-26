using System;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    public event Action OnCoinTake;
    
    [Header("Movement Settings")]
    [SerializeField] private float rotationSpeed = 100f;   // скорость вращения
    [SerializeField] private float floatAmplitude = 0.25f; // амплитуда подъема/спуска
    [SerializeField] private float floatSpeed = 2f;        // скорость покачивания

    [Header("Pickup Settings")]
    [SerializeField] private GameObject pickupParticle;    // партиклы при сборе
    [SerializeField] private AudioClip pickupSound;        // звук при сборе
    [SerializeField] private float destroyDelay = 2f;      // через сколько удалить партикл

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;

        OnCoinTake += UIController.Instance.HandleCoinText;
    }

    private void Update()
    {
        // Вращение по Y
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // Движение вверх-вниз (синус)
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out var player))
        {
            // Спавним партикл
            if (pickupParticle != null)
            {
                GameObject p = Instantiate(pickupParticle, transform.position, Quaternion.identity);
                Destroy(p, destroyDelay);
            }

            // Проигрываем звук
            if (pickupSound != null)
            {
                MusicController.Instance.PlaySpecificSound(pickupSound);
            }

            InventoryController.Instance.coinsCount++;
            OnCoinTake?.Invoke();
            
            // Отключаем монету
            gameObject.SetActive(false);
        }
    }
}