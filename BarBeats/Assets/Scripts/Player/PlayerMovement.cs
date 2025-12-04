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
    [SerializeField] private float followThroughStrength = 1.25f;
    [SerializeField] private float followThroughTime = .05f;
    [SerializeField] private float followThroughDirectionFactor = 1.5f;
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashTime = 0.12f;
    [SerializeField] private float dashCooldown = 1f;

    private Rigidbody2D _rigidbody;
    private Health health;
    private Vector2 _moveDirection;
    private PlayerState _state;
    private Vector2 _velocity = Vector2.zero;
    private bool _movementControl = true;
    bool isDashing = false;
    float nextDashTime = 0f;
    Vector2 cachedMoveInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
    }

    void Update()
    {
        float hx = (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed ? 1f : 0f)
                 - (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed ? 1f : 0f);
        float hy = (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed ? 1f : 0f)
                 - (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed ? 1f : 0f);
        cachedMoveInput = new Vector2(hx, hy);

        if (!isDashing && Time.time >= nextDashTime && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            StartCoroutine(Dash());
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.PlayerManager.IsGameOver()) return;

        if (isDashing)
        {
            // Flip based on input used for the dash so sprite faces correct direction
            if (cachedMoveInput.x < 0 && !_sr.flipX) _sr.flipX = true;
            else if (cachedMoveInput.x > 0 && _sr.flipX) _sr.flipX = false;

            // Skip normal movement while dashing
            return;
        }

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

    // Idk where else to put this so I put it here
    public void Pause(InputAction.CallbackContext context)
    {
        GameManager.UiManager.Pause();
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
        }
        else if (_velocity.x > 0 && _sr.flipX)
        {
            _sr.flipX = false;
        }
    }
    
    IEnumerator Dash()
    {
        isDashing = true;
        health.SetInvulnerable();
        nextDashTime = Time.time + dashCooldown;

        // Use cached keyboard input (set in Update)
        Vector2 dir = cachedMoveInput.sqrMagnitude > 0.001f ? cachedMoveInput.normalized : (Vector2)transform.right;

        _rigidbody.linearVelocity = dir * dashSpeed;

        // Optionally disable your movement control so other movement code doesn't interfere
        _movementControl = false;

        yield return new WaitForSeconds(dashTime);

        // Stop dash velocity and restore movement control
        _rigidbody.linearVelocity = Vector2.zero;
        _velocity = Vector2.zero;
        _movementControl = true;
        isDashing = false;
        health.SetVulnerable();
    }
}
