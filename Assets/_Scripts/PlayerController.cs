using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private Vector2 movementInput;
    private Vector2 lookInput;
    private PlayerControls controlsPlayer;

    public float speed = 5f;
    public float rotationSpeed = 15f;

    private void Awake()
    {
        controlsPlayer = new PlayerControls();
        characterController = GetComponent<CharacterController>();

        // �������� ���� ��� ��������� �������
    }

    private void Update()
    {
        // ��������� �������� � ��������� ������ ��� ������� �����
        if (movementInput.magnitude > 0f)
        {
            // ��������� �������� � ���������
            Vector3 moveDirection = new Vector3(movementInput.x, 0f, movementInput.y).normalized;
            Vector3 move = transform.TransformDirection(moveDirection) * speed * Time.deltaTime;
            characterController.Move(move);
        }

        // ������� �������� ������ ��� Y (������������ ���) �� ������ ����� ���� ��� ���������
        Vector3 rotation = new Vector3(0f, lookInput.x, 0f) * rotationSpeed * Time.deltaTime;
        transform.Rotate(rotation);
    }

    private void OnEnable()
    {
        controlsPlayer.Enable();

        // ������������� �� ������� �������� Move
        controlsPlayer.Player.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        controlsPlayer.Player.Move.canceled += ctx => movementInput = Vector2.zero;

        // ������������� �� ������� �������� Look
        controlsPlayer.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controlsPlayer.Player.Look.canceled += ctx => lookInput = Vector2.zero;
    }

    private void OnDisable()
    {
        // ������������ �� ������� �������� Move
        controlsPlayer.Player.Move.performed -= ctx => movementInput = ctx.ReadValue<Vector2>();
        controlsPlayer.Player.Move.canceled -= ctx => movementInput = Vector2.zero;

        // ������������ �� ������� �������� Look
        controlsPlayer.Player.Look.performed -= ctx => lookInput = ctx.ReadValue<Vector2>();
        controlsPlayer.Player.Look.canceled -= ctx => lookInput = Vector2.zero;
        controlsPlayer.Disable();
    }
}