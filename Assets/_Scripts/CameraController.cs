using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform cameraPivot;
    private Transform playerTransform;

    public float cameraFollowSpeed = 5f;
    public float smoothTime = 0.3f; // ����� ���������� ��� ����������
    public float followDistance = 2f; // ����������, �� ������� ������ ������ ���������

    private Vector3 cameraVelocity; // ��� SmoothDamp

    private void Awake()
    {
        // ������� �������� ����� � �������� ��������� ������� ������
        cameraPivot = new GameObject("CameraPivot").transform;
        cameraPivot.SetParent(transform);
        cameraPivot.localPosition = new Vector3(0f, 2f, 0f); // ���������� ������ ������

        // ����������� ������ � ������
        Camera.main.transform.SetParent(cameraPivot);
        Camera.main.transform.localPosition = new Vector3(0f, 0f, 0.2f); // ���������� ���������� �� ������

        // �������� ������ �� ��������� ������
        playerTransform = transform;
    }

    private void LateUpdate()
    {
        FollowPlayerWithCamera();
    }

    private void FollowPlayerWithCamera()
    {
        // ��������� �������� ������� ��� ������
        Vector3 targetPosition = playerTransform.position + (-playerTransform.forward * 5f) + new Vector3(0f, 1.5f, 0f);

        // ������ ������������� � �������� ������� � �������������� SmoothDamp
        cameraPivot.position = Vector3.SmoothDamp(cameraPivot.position, targetPosition, ref cameraVelocity, smoothTime);

        // ��������� ���������� ����� ������� � �������
        float distanceToPlayer = Vector3.Distance(cameraPivot.position, playerTransform.position);

        // ���� ���������� ������ followDistance, ������ �������� ���������
        if (distanceToPlayer > followDistance)
        {
            // �������� ����������� �� ������ � ������
            Vector3 directionToPlayer = (playerTransform.position - cameraPivot.position).normalized;

            // ������������ ����� �������, �������� followDistance
            Vector3 newPosition = playerTransform.position - directionToPlayer * followDistance;

            // ������ ������������� � ����� �������
            cameraPivot.position = Vector3.SmoothDamp(cameraPivot.position, newPosition, ref cameraVelocity, smoothTime);
        }
    }
}