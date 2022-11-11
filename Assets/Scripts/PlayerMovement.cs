using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12f;
    void Update()
    {

        // Movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        float y = Input.GetAxis("Height");
        float r = Input.GetAxis("RotationAxis");
        
        Vector3 move = transform.right * x + transform.forward * z + transform.up * y;

        controller.Move(move * speed * Time.deltaTime);

        this.gameObject.transform.RotateAround(transform.position, transform.up, r * Time.deltaTime * 90f);


    }
}