using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;

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
    //private float attackTimer = 0f;

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
            // Получаем направление взгляда персонажа в мировых координатах
            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraRight = Camera.main.transform.right;

            // Убираем y-компоненту, чтобы избежать движения по вертикали
            cameraForward.y = 0f;
            cameraRight.y = 0f;

            // Нормализуем векторы направлений
            cameraForward.Normalize();
            cameraRight.Normalize();

            // Считаем направление движения в зависимости от ввода и направления взгляда
            Vector3 moveDirection = (cameraForward * movementInput.y + cameraRight * movementInput.x).normalized;

            // Применяем движение к персонажу
            Vector3 move = moveDirection * speed * Time.deltaTime;
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
    }

    private void HandleAttack()
    {
        //// Если происходит атака, начинаем отсчет времени атаки и проигрываем анимацию
        //if (isAttacking)
        //{
        //    attackTimer += Time.deltaTime;

        //    // По завершении анимации сбрасываем состояние атаки
        //    if (attackTimer >= attackDuration)
        //    {
        //        isAttacking = false;
        //        attackTimer = 0f;
        //        animator.SetBool("LightAttack", false);
        //    }
        //}

        if (isAttacking)
        {
            AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (animatorStateInfo.IsName("LightAttack") && animatorStateInfo.normalizedTime >= 1f)
            {
                isAttacking = false;
                animator.SetBool("LightAttack", false);
            }
            //else if (!animatorStateInfo.IsName("LightAttack") && animator.GetBool("LightAttack"))
            //{
            //    animator.SetBool("LightAttack", false);
            //}

            if (animatorStateInfo.IsName("HeavyAttack") && animatorStateInfo.normalizedTime >= 1f)
            {
                isAttacking = false;
                animator.SetBool("HeavyAttack", false);
            }
            //else if (!animatorStateInfo.IsName("HeavyAttack") && animator.GetBool("HeavyAttack"))
            //{
            //    animator.SetBool("HeavyAttack", false);
            //}
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
        controlsPlayer.Player.Attack.performed += ctx => StartLightAttack();
        controlsPlayer.Player.SecondAttack.performed += ctx => StartHeavyAttack();
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
        controlsPlayer.Player.Attack.performed -= ctx => StartLightAttack();
        controlsPlayer.Player.SecondAttack.performed -= ctx => StartHeavyAttack();

        controlsPlayer.Disable();
    }

    private void StartLightAttack()
    {
        // Если не находимся в процессе атаки, начинаем атаку
        if (!isAttacking)
        {
            // Запуск анимации атаки
            animator.SetBool("LightAttack", true);

            // Запуск атаки
            isAttacking = true;
        }
    }

    private void StartHeavyAttack()
    {
        // Если не находимся в процессе атаки, начинаем атаку
        if (!isAttacking)
        {
            // Запуск анимации атаки
            animator.SetBool("HeavyAttack", true);

            // Запуск атаки
            isAttacking = true;
        }
    }
}