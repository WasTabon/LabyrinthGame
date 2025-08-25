using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Joystick joystick;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Camera Follow")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 10, -5);
    [SerializeField] private float cameraFollowSpeed = 5f;

    [Header("Camera Limits")]
    [SerializeField] private Transform cameraZoneCenter;   // центр зоны
    [SerializeField] private Vector2 cameraZoneSize = new Vector2(40, 40); // размеры зоны
    [SerializeField] private float reattachDistance = 5f; // дистанция, после которой камера "подцепится" с offset/2

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string runBoolName = "Run";

    private Rigidbody rb;

    private enum CameraState { Normal, StuckAtBorder, FollowReduced }
    private CameraState cameraState = CameraState.Normal;

    private Vector3 lastClampedPosition; // позиция камеры на границе при выходе

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector2 input = joystick.Direction;
        Vector3 move = new Vector3(input.x, 0f, input.y);

        rb.velocity = move * moveSpeed + new Vector3(0, rb.velocity.y, 0);

        if (move.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move, Vector3.up);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }

        if (animator != null)
        {
            bool isRunning = move.sqrMagnitude > 0.01f;
            animator.SetBool(runBoolName, isRunning);
        }
    }

    private void LateUpdate()
    {
        if (cameraTransform == null || cameraZoneCenter == null) return;

        // вычисляем границы
        Vector3 halfSize = new Vector3(cameraZoneSize.x / 2f, 0, cameraZoneSize.y / 2f);
        Vector3 minBounds = cameraZoneCenter.position - halfSize;
        Vector3 maxBounds = cameraZoneCenter.position + halfSize;

        // обычная цель
        Vector3 desiredPos = transform.position + cameraOffset;

        // "зажатая" цель в границах
        Vector3 clampedPos = new Vector3(
            Mathf.Clamp(desiredPos.x, minBounds.x, maxBounds.x),
            desiredPos.y,
            Mathf.Clamp(desiredPos.z, minBounds.z, maxBounds.z)
        );

        bool outsideX = transform.position.x < minBounds.x || transform.position.x > maxBounds.x;
        bool outsideZ = transform.position.z < minBounds.z || transform.position.z > maxBounds.z;
        bool playerOutside = outsideX || outsideZ;

        switch (cameraState)
        {
            case CameraState.Normal:
                if (playerOutside)
                {
                    lastClampedPosition = clampedPos; // запоминаем точку границы
                    cameraState = CameraState.StuckAtBorder;
                }
                cameraTransform.position = Vector3.Lerp(cameraTransform.position, clampedPos, cameraFollowSpeed * Time.deltaTime);
                break;

            case CameraState.StuckAtBorder:
                // остаёмся на границе
                cameraTransform.position = Vector3.Lerp(cameraTransform.position, lastClampedPosition, cameraFollowSpeed * Time.deltaTime);

                // если игрок отбежал достаточно далеко — разрешаем следить за ним с уменьшенным offset
                if (Vector3.Distance(transform.position, lastClampedPosition) > reattachDistance)
                {
                    cameraState = CameraState.FollowReduced;
                }

                // если вернулся внутрь — сразу в нормальный режим
                if (!playerOutside)
                {
                    cameraState = CameraState.Normal;
                }
                break;

            case CameraState.FollowReduced:
                // теперь без Clamp! Камера может выходить за границы
                Vector3 reducedOffset = cameraOffset * 0.5f;
                Vector3 reducedTarget = transform.position + reducedOffset;

                cameraTransform.position = Vector3.Lerp(cameraTransform.position, reducedTarget, cameraFollowSpeed * Time.deltaTime);

                // если игрок вернулся внутрь — вернуться к Normal
                if (!playerOutside)
                {
                    cameraState = CameraState.Normal;
                }
                break;
        }
    }

    private void OnDrawGizmos()
    {
        if (cameraZoneCenter == null) return;

        Gizmos.color = Color.green;
        Vector3 size = new Vector3(cameraZoneSize.x, 0.1f, cameraZoneSize.y);
        Gizmos.DrawWireCube(cameraZoneCenter.position, size);
    }
}
