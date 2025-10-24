using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Public vars
    public float speed = 5f;
    
    private Rigidbody2D _rigidbody;
    private Vector2 _moveDirection;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 currentPos = _rigidbody.position;
        Vector2 movement = _moveDirection * speed * Time.fixedDeltaTime;
        _rigidbody.MovePosition(currentPos + movement);
    }

    public void Move(InputAction.CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector2>();
        _moveDirection.Normalize();
    }
    
}
