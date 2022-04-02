using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class Movement : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 10f;
    [SerializeField] private float playerRun = 5f;
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private float jumpHeight = 4f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private bool usePhysics = true;
    [SerializeField] private float rotationSpeed;
    float turnSmoothVelocity;
    public LayerMask layerMask;

    private Camera _mainCamera;
    public Transform cam;
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
    }
    private void Update()
    {
        // if (ground && playerVelocity.y < 0){
        //     playerVelocity.y = 0f;
        // }

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
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
            playerVelocity.y += gravityValue * Time.deltaTime;
            Vector3 targetJump = transform.position + playerVelocity * Time.deltaTime;
            Move(targetJump);
            playerVelocity.y = 0;
        }else{
            _animator.SetBool(IsJumping, false);
        }

         if(_controls.Player.Running.IsPressed()){
             Debug.Log("masuk running");
            _animator.SetBool(IsRunning, true);
            Vector3 target = HandleInput(input,playerRun);
            MovePhysics(target);
        }else{
            _animator.SetBool(IsRunning, false);
        }

    }

    private void FixedUpdate()
    {

        // if (ground && playerVelocity.y < 0){
        //     playerVelocity.y = 0f;
        // }

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
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
            playerVelocity.y += gravityValue * Time.deltaTime;
            Vector3 targetJump = transform.position + playerVelocity * Time.deltaTime;
            MovePhysics(targetJump);
            playerVelocity.y = 0;
        }else{
            _animator.SetBool(IsJumping, false);
        }

        if(_controls.Player.Running.IsPressed()){
            Debug.Log("masuk running");
            _animator.SetBool(IsRunning, true);
            Vector3 target = HandleInput(input,playerRun);
            MovePhysics(target);
        }else{
            Debug.Log("tidak running");
            _animator.SetBool(IsRunning, false);
        }
      
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
