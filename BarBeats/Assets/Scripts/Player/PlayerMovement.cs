using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Public vars
    [SerializeField] float speed = 5f;

    private Rigidbody2D _rigidbody;
    private Vector2 _moveDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 currentPos = _rigidbody.position;
        Vector2 movement = _moveDirection * speed * Time.fixedDeltaTime;
        _rigidbody.MovePosition(currentPos + movement);

        // TODO: remove
        if (transform.position.x > 0)
        {
            AkEventList.instance.medley_bgm_choice_march.SetValue();
        }
        else
        {
            AkEventList.instance.medley_bgm_choice_western.SetValue();
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector2>();
        _moveDirection.Normalize();
    }

}
