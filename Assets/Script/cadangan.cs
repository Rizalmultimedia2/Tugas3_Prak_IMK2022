using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class cadangan : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private bool usePhysics = true;
    [SerializeField] private float rotationSpeed = 4f;

    private Camera _mainCamera;
    private Rigidbody _rb;
    private Controls _controls;
    private Vector3 playerVelocity;
    private Animator _animator;
    private static readonly int IsWalking = Animator.StringToHash("isWalking");

    private void Awake()
    {
        _controls = new Controls();
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _controls.Enable();
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        _controls.Disable();
    }

    private void Start()
    {
        _mainCamera = Camera.main;
        _rb = gameObject.GetComponent<Rigidbody>();
        _animator = gameObject.GetComponentInChildren<Animator>();
    }
    private void Update()
    {

        playerVelocity.y = 0f;

        if (usePhysics)
        {
            return;
        }
        
        Vector2 input = _controls.Player.Move.ReadValue<Vector2>();

        if (_controls.Player.Move.IsPressed())
        {
            _animator.SetBool(IsWalking, true);
            Vector3 target = HandleInput(input);
            Move(target);
        }
        else
            _animator.SetBool(IsWalking, false);

        if (_controls.Player.Jump.IsPressed())
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            playerVelocity.y += gravityValue * Time.deltaTime;
            Vector3 targetJump = transform.position + playerVelocity * Time.deltaTime;
            Move(targetJump);
        }
        // if(input != Vector2.zero){
        //     float targetAngle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;
        //     Quaternion rotation = Quaternion.Euler(0f,targetAngle,0f);
        //     transform.rotation = Quaternion.Lerp(transform.rotation,rotation,Time.deltaTime*rotationSpeed);
        // }

    }

    private void FixedUpdate()
    {
        
            playerVelocity.y = 0f;

        if (!usePhysics)
        {
            return;
        }

        Vector2 input = _controls.Player.Move.ReadValue<Vector2>();

        if (_controls.Player.Move.IsPressed())
        {
            _animator.SetBool(IsWalking, true);
            Vector3 target = HandleInput(input);
            MovePhysics(target);
        }
        else
            _animator.SetBool(IsWalking, false);

        if (_controls.Player.Jump.IsPressed())
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            playerVelocity.y += gravityValue * Time.deltaTime;
            Vector3 targetJump = transform.position + playerVelocity * Time.deltaTime;
            MovePhysics(targetJump);
        }
        // if(input != Vector2.zero){
        //     float targetAngle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;
        //     Quaternion rotation = Quaternion.Euler(0f,targetAngle,0f);
        //     transform.rotation = Quaternion.Lerp(transform.rotation,rotation,Time.deltaTime*rotationSpeed);
        // }
    }

    private Vector3 HandleInput(Vector2 input)
    {
        Vector3 forward = _mainCamera.transform.forward;
        Vector3 right = _mainCamera.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 direction = right * input.x + forward * input.y;

        return transform.position + direction * playerSpeed * Time.deltaTime;
    }

    private void Move(Vector3 target)
    {
        transform.position = target;
    }

    private void MovePhysics(Vector3 target)
    {
        _rb.MovePosition(target);
    }
}
