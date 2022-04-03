using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class Movement : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 10f;
    [SerializeField] private float jumpHeight = 4f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private bool usePhysics = true;
    [SerializeField] private float rotationSpeed;
    float turnSmoothVelocity;

    private Camera _mainCamera;
    private Rigidbody _rb;
    private Controls _controls;
    private Vector3 playerVelocity;
    private Animator _animator;
    private CharacterController controller;
    private static readonly int IsWalking = Animator.StringToHash("isWalking");
    private static readonly int IsJumping = Animator.StringToHash("isJumping");
    private static readonly int IsRunning = Animator.StringToHash("isRunning");


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
        controller = gameObject.GetComponent<CharacterController>();
    }
    private void Update()
    {
        if (usePhysics)
        {
            return;
        }

        Vector2 input = _controls.Player.Move.ReadValue<Vector2>();

        if (_controls.Player.Move.IsPressed())
        {
            _animator.SetBool(IsWalking, true);
            Vector3 target = HandleInput(input,playerSpeed);
            Move(target);
        }
        else
            _animator.SetBool(IsWalking, false);

        if (_controls.Player.Jump.IsPressed())
        {
            _animator.SetBool(IsJumping, true);
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            playerVelocity.y += gravityValue * Time.deltaTime;
            Vector3 targetJump = transform.position + playerVelocity * Time.deltaTime;
            Move(targetJump);
            
        }else{
            _animator.SetBool(IsJumping, false);
        }

         if(_controls.Player.Running.IsPressed()){
            _animator.SetBool(IsRunning, true);
            ShakingCamera.Instance.ShakeCamera(3f);
            Vector3 target = HandleInput(input,playerSpeed + 5f);
            MovePhysics(target);
        }else{
            _animator.SetBool(IsRunning, false);
            ShakingCamera.Instance.ShakeCamera(0f);
        }
        playerVelocity.y = 0.9f;

    }

    private void FixedUpdate()
    {

        if (!usePhysics)
        {
            return;
        }

        Vector2 input = _controls.Player.Move.ReadValue<Vector2>();

        if (_controls.Player.Move.IsPressed())
        {
            _animator.SetBool(IsWalking, true);
            Vector3 target = HandleInput(input,playerSpeed);
            MovePhysics(target);
        }
        else
            _animator.SetBool(IsWalking, false);

        if (_controls.Player.Jump.IsPressed())
        {
            _animator.SetBool(IsJumping, true);
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            playerVelocity.y += gravityValue * Time.deltaTime;
            Vector3 targetJump = transform.position + playerVelocity * Time.deltaTime;
            MovePhysics(targetJump);
            
        }else{
            _animator.SetBool(IsJumping, false);
        }

        if(_controls.Player.Running.IsPressed()){
            _animator.SetBool(IsRunning, true);
            ShakingCamera.Instance.ShakeCamera(1.5f);
            Vector3 target = HandleInput(input,playerSpeed + 5f);
            MovePhysics(target);
        }else{
            _animator.SetBool(IsRunning, false);
            ShakingCamera.Instance.ShakeCamera(0f);
        }
        playerVelocity.y = 0;
      
    }

    private Vector3 HandleInput(Vector2 input, float speed)
    {
        Vector3 forward = _mainCamera.transform.forward;
        Vector3 right = _mainCamera.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 direction = right * input.x + forward * input.y;
       
        if(direction != Vector3.zero){
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        return transform.position + direction * speed * Time.deltaTime;
    }

    private void Move(Vector3 target)
    {
        transform.position = target;

    }

    private void MovePhysics(Vector3 target)
    {
        transform.position = target;
    }
}
