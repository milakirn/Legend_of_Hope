using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform cameraPivot;
    private Transform playerTransform;

    public float cameraFollowSpeed = 5f;
    public float smoothTime = 0.3f; // Время замедления при следовании
    public float followDistance = 2f; // Расстояние, на котором камера начнет следовать

    private Vector3 cameraVelocity; // Для SmoothDamp

    private void Awake()
    {
        // Создаем камерный пивот в качестве дочернего объекта игрока
        cameraPivot = new GameObject("CameraPivot").transform;
        cameraPivot.SetParent(transform);
        cameraPivot.localPosition = new Vector3(0f, 2f, 0f); // Регулируем высоту камеры

        // Прикрепляем камеру к пивоту
        Camera.main.transform.SetParent(cameraPivot);
        Camera.main.transform.localPosition = new Vector3(0f, 0f, 0.2f); // Регулируем расстояние от игрока

        // Получаем ссылку на трансформ игрока
        playerTransform = transform;
    }

    private void LateUpdate()
    {
        FollowPlayerWithCamera();
    }

    private void FollowPlayerWithCamera()
    {
        // Вычисляем желаемую позицию для камеры
        Vector3 targetPosition = playerTransform.position + (-playerTransform.forward * 5f) + new Vector3(0f, 1.5f, 0f);

        // Плавно интерполируем к желаемой позиции с использованием SmoothDamp
        cameraPivot.position = Vector3.SmoothDamp(cameraPivot.position, targetPosition, ref cameraVelocity, smoothTime);

        // Проверяем расстояние между камерой и игроком
        float distanceToPlayer = Vector3.Distance(cameraPivot.position, playerTransform.position);

        // Если расстояние больше followDistance, камера начинает следовать
        if (distanceToPlayer > followDistance)
        {
            // Получаем направление от камеры к игроку
            Vector3 directionToPlayer = (playerTransform.position - cameraPivot.position).normalized;

            // Рассчитываем новую позицию, учитывая followDistance
            Vector3 newPosition = playerTransform.position - directionToPlayer * followDistance;

            // Плавно интерполируем к новой позиции
            cameraPivot.position = Vector3.SmoothDamp(cameraPivot.position, newPosition, ref cameraVelocity, smoothTime);
        }
    }
}