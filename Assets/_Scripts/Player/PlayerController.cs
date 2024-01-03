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
    public float attackDuration = 1.0f; // Длительность атаки (время проигрывания анимации)

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
        // Применяем движение к персонажу только при наличии ввода
        if (movementInput.magnitude > 0f)
        {
            // Применяем движение к персонажу
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
        // Обработка атаки
        HandleAttack();

        transform.rotation = mainCamera.transform.rotation;

        //// Вращаем персонаж вокруг оси Y (вертикальная ось) на основе ввода мыши или джойстика
        //Vector3 rotation = new Vector3(0f, lookInput.x, 0f) * rotationSpeed * Time.deltaTime;
        //transform.Rotate(rotation);
    }

    private void HandleAttack()
    {
        // Если происходит атака, начинаем отсчет времени атаки и проигрываем анимацию
        if (isAttacking)
        {
            attackTimer += Time.deltaTime;

            // По завершении анимации сбрасываем состояние атаки
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

        // Подписываемся на событие действия Move
        controlsPlayer.Player.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        controlsPlayer.Player.Move.canceled += ctx => movementInput = Vector2.zero;

        // Подписываемся на событие действия Look
        controlsPlayer.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controlsPlayer.Player.Look.canceled += ctx => lookInput = Vector2.zero;

        // Подписываемся на событие действия Attack
        controlsPlayer.Player.Attack.performed += ctx => StartAttack();
    }

    private void OnDisable()
    {
        // Отписываемся от события действия Move
        controlsPlayer.Player.Move.performed -= ctx => movementInput = ctx.ReadValue<Vector2>();
        controlsPlayer.Player.Move.canceled -= ctx => movementInput = Vector2.zero;

        // Отписываемся от события действия Look
        controlsPlayer.Player.Look.performed -= ctx => lookInput = ctx.ReadValue<Vector2>();
        controlsPlayer.Player.Look.canceled -= ctx => lookInput = Vector2.zero;

        // Отписываемся от события действия Attack
        controlsPlayer.Player.Attack.performed -= ctx => StartAttack();

        controlsPlayer.Disable();
    }

    private void StartAttack()
    {
        // Если не находимся в процессе атаки, начинаем атаку
        if (!isAttacking)
        {
            // Запуск анимации атаки
            animator.SetBool("AttackBool", true);

            // Запуск атаки
            isAttacking = true;
        }
    }
}