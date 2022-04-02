using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private int speed = 10;
    private bool usePhysics = true;
    private Camera mainCamera;
    private Rigidbody rb;
    private Controls controls;

    private void Awake() {
        controls = new Controls();
    }

    private void OnEnable() {
        controls.Enable();
    }

    private void OnDisable() {
        controls.Disable();
    }

    // Start is called before the first frame update
    private void Start()
    {
        mainCamera = Camera.main;
        rb = gameObject.GetComponent<Rigidbody>();   
    }

    // Update is called once per frame
    private void Update()
    {
        if(usePhysics){
            return;
        }
       
        if(controls.Player.Move.IsPressed()){
            Vector2 input = controls.Player.Move.ReadValue<Vector2>();
            Vector3 target = HandleInput(input);
            Move(target);
        }
    }

    private void FixedUpdate()
    {
        if(!usePhysics){
            return;
        }

       if(controls.Player.Move.IsPressed()){
            Vector2 input = controls.Player.Move.ReadValue<Vector2>();
            Vector3 target = HandleInput(input);
            MovePhysics(target);
        }
    }
    private Vector3 HandleInput(Vector2 input){
        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;

        forward.y = 0;
        right.y = 0;

        right.Normalize();
        forward.Normalize();
        
        Vector3 direction = forward * input.y + right * input.x;

        return transform.position = transform.position + direction * speed * Time.deltaTime;
    }

    private void Move(Vector3 target){
       transform.position = target;
    }

    private void MovePhysics(Vector3 taget){
        rb.MovePosition(taget);
    }
}
