using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public int a = 10;
    public Camera mainCamera;
    // Start is called before the first frame update
    private void Start()
    {
        mainCamera = Camera.main;   
    }

    // Update is called once per frame
    private void Update()
    {
        if(Input.GetKey(KeyCode.W)){
            Move(Vector2.up);
        }else if(Input.GetKey(KeyCode.S)){
            Move(Vector2.down);
        }else if(Input.GetKey(KeyCode.A)){
            Move(Vector2.left);
        }else if(Input.GetKey(KeyCode.D)){
            Move(Vector2.right);
        }
    }

    private async void Move(Vector2 input){
        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;

        forward.y = 0;
        right.y = 0;

        right.Normalize();
        forward.Normalize();
        
        Vector3 direction = forward * input.y + right * input.x;

        transform.position = transform.position + direction * a * Time.deltaTime;

    }
}
