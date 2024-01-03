using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private Vector2 movementInput;
    private Vector2 lookInput;
    private PlayerControls controlsPlayer;
    private PlayerSpawn playerSpawn;
    private Animator animator;

    [SerializeField]
    private Camera mainCamera;

    public float speed = 1f;
    public float rotationSpeed = 2f;
    public float attackDuration = 1.0f; // ������������ ����� (����� ������������ ��������)

    private bool isAttacking = false;
    private float attackTimer = 0f;

    private void Awake()
    {
        controlsPlayer = new PlayerControls();
        characterController = GetComponent<CharacterController>();
        playerSpawn = GetComponent<PlayerSpawn>();
        animator = GetComponent<Animator>();

        playerSpawn.SpawnPlayer();
    }

    private void FixedUpdate()
    {
        // ��������� �������� � ��������� ������ ��� ������� �����
        if (movementInput.magnitude > 0f)
        {
            // ��������� �������� � ���������
            Vector3 moveDirection = new Vector3(movementInput.x, 0f, movementInput.y).normalized;
            Vector3 move = transform.TransformDirection(moveDirection) * speed * Time.deltaTime;
            characterController.Move(move);

            animator.SetBool("IsMoving", true);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
    }

    private void Update()
    {
        // ��������� �����
        HandleAttack();

        transform.rotation = mainCamera.transform.rotation;

        //// ������� �������� ������ ��� Y (������������ ���) �� ������ ����� ���� ��� ���������
        //Vector3 rotation = new Vector3(0f, lookInput.x, 0f) * rotationSpeed * Time.deltaTime;
        //transform.Rotate(rotation);
    }

    private void HandleAttack()
    {
        // ���� ���������� �����, �������� ������ ������� ����� � ����������� ��������
        if (isAttacking)
        {
            attackTimer += Time.deltaTime;

            // �� ���������� �������� ���������� ��������� �����
            if (attackTimer >= attackDuration)
            {
                isAttacking = false;
                attackTimer = 0f;
                animator.SetBool("AttackBool", false);
            }
        }
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

        // ������������� �� ������� �������� Attack
        controlsPlayer.Player.Attack.performed += ctx => StartAttack();
    }

    private void OnDisable()
    {
        // ������������ �� ������� �������� Move
        controlsPlayer.Player.Move.performed -= ctx => movementInput = ctx.ReadValue<Vector2>();
        controlsPlayer.Player.Move.canceled -= ctx => movementInput = Vector2.zero;

        // ������������ �� ������� �������� Look
        controlsPlayer.Player.Look.performed -= ctx => lookInput = ctx.ReadValue<Vector2>();
        controlsPlayer.Player.Look.canceled -= ctx => lookInput = Vector2.zero;

        // ������������ �� ������� �������� Attack
        controlsPlayer.Player.Attack.performed -= ctx => StartAttack();

        controlsPlayer.Disable();
    }

    private void StartAttack()
    {
        // ���� �� ��������� � �������� �����, �������� �����
        if (!isAttacking)
        {
            // ������ �������� �����
            animator.SetBool("AttackBool", true);

            // ������ �����
            isAttacking = true;
        }
    }
}