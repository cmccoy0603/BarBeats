using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Public vars
    [SerializeField] private float speed = 5f;
    [SerializeField] private float followThroughStrength = 5f;
    [SerializeField] private float followThroughTime = .1f;
    [SerializeField] private float followThroughDirectionFactor = 1.25f;
    [SerializeField] private SpriteRenderer _sr;

    private Rigidbody2D _rigidbody;
    private Vector2 _moveDirection;
    private PlayerState _state;
    private Vector2 _velocity = Vector2.zero;
    private bool _movementControl = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (GameManager.PlayerManager.IsGameOver()) return;
        
        Vector2 currentPos = _rigidbody.position;

        _state = CalculateState();

        switch (_state)
        {
            // Make sure the player is not moving
            case PlayerState.STILL:
                _velocity = Vector2.zero;
                break;
            case PlayerState.MOVING:
                _velocity = _moveDirection * speed;
                break;
            case PlayerState.NO_CONTROL:
                break;
        }
        
        FlipPlayer();
        
        _rigidbody.MovePosition(currentPos + (_velocity * Time.fixedDeltaTime));

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

    // I wanted to put a state system together, so this is the beginnings of how to get the current player state
    private PlayerState CalculateState()
    {
        if (!_movementControl)
        {
            return PlayerState.NO_CONTROL;
        }

        return _moveDirection != Vector2.zero ? PlayerState.MOVING : PlayerState.STILL;
    }

    public void Move(InputAction.CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector2>();
        _moveDirection.Normalize();
    }

    public void FollowThrough(Vector2 direction)
    {
        // Get the current players velocity
        Vector2 velocityNorm = _velocity.normalized;
        float directionalStrength = Vector2.Dot(direction, velocityNorm);
        float directionMult = directionalStrength > 0
            ? directionalStrength
            : 0;
        
        _velocity = direction * followThroughStrength * ((1 + directionMult) * followThroughDirectionFactor);
        StartCoroutine(GiveBackControl(followThroughTime));
    }

    IEnumerator GiveBackControl(float delay)
    {
        _movementControl = false;
        yield return new WaitForSeconds(delay);
        _movementControl = true;
    }

    private void FlipPlayer()
    {
        if (_velocity.x < 0 && !_sr.flipX)
        {
            _sr.flipX = true;
        } else if (_velocity.x > 0 && _sr.flipX)
        {
            _sr.flipX = false;
        }
    }
}
