using System;
using Enums;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    // All stuff we get from serialize field
    [SerializeField] private Animator animator;
    public float attackRange = 1;
    public float attackSpread = 1;
    public float weaponDamage = 10;
    public float meleeSpeed = .25f;
    public float lifesteal = 0f;

    private Camera cam;
    private Health playerHealth;

    // The holder the contains the weapon
    [SerializeField] private WeaponHolder weaponHolder;

    // All of our silly input stuff
    private InputAction _mousePositionAction;
    private InputAction _joystickPositionAction;
    private InputAction _keysPositionAction;
    private InputDevice _attackDevice;
    private bool _attackedThisFrame = false;
    private bool _threwThisFrame = false;


    private void Awake()
    {
        cam = Camera.main;
        playerHealth = GetComponent<Health>();
        _mousePositionAction = InputSystem.actions.FindAction("MousePos");
        _joystickPositionAction = InputSystem.actions.FindAction("StickPos");
        _keysPositionAction = InputSystem.actions.FindAction("KeysPos");
    }

    private void Update()
    {
        if (!_attackedThisFrame || GameManager.PlayerManager.IsGameOver()) return;

        if (!_threwThisFrame)
        {
            Attack(_attackDevice);
        }
        else
        {
            Throw(_attackDevice);
        }
        
    }

    public void PreAttack(InputAction.CallbackContext context)
    {
        _attackDevice = context.control.device;
        _attackedThisFrame = true;
    }

    // So we can time our throw to be on frame
    public void PreThrow(InputAction.CallbackContext context)
    {
        _attackDevice = context.control.device;
        _attackedThisFrame = true;
        _threwThisFrame = true;
    }
    
    public void Attack(InputDevice device)
    {
        _attackedThisFrame = false;
        Vector2 aimVector = GetAttackDirection(device).normalized;
        float angle = Mathf.Atan2(aimVector.y, aimVector.x);
        
        if (GameManager.GameState != GameState.PLAYING)
        {
            return;
        }
        
        // Actually try to attack
        if (!weaponHolder.Attack(angle)) return;

        animator.SetTrigger("Attack");
        // Player follow through
        GameManager.PlayerManager.MovePlayer(aimVector);
    }

    void Throw(InputDevice device)
    {
        _attackedThisFrame = false;
        _threwThisFrame = false;
        
        // Can we actually throw?
        if (!weaponHolder.HasWeapon()) return;
        
        Vector2 aimVector = GetAttackDirection(device).normalized;
        float angle = Mathf.Atan2(aimVector.y, aimVector.x);
        weaponHolder.Throw(angle);
    }

    private Vector2 GetAttackDirection(InputDevice device)
    {
        Vector3 pointerPos = Vector3.zero;

        if (device is Mouse)
        {
            pointerPos = _mousePositionAction.ReadValue<Vector2>();
            pointerPos.z = cam.nearClipPlane;
            Vector3 point = cam.ScreenToWorldPoint(pointerPos);

            // Now we get the vector from the player to the point (aim direction)
            pointerPos = point - transform.position;
        }
        else if (device is Gamepad)
        {
            pointerPos = _joystickPositionAction.ReadValue<Vector2>();
        }
        else if (device is Keyboard)
        {
            try
            {
                pointerPos = _keysPositionAction.ReadValue<Vector2>();
            }
            catch (Exception e)
            {
                Debug.Log("Why did I get an error >:-( " + e.Message);
            }

            Debug.Log("Keyboard position?" + pointerPos);
        }

        return pointerPos;
    }

    public void OnKillEnemy()
    {
        // The below line is only called so that lifesteal works. If any conflicting code is added to in the future, then feel free to remove this, just make sure that lifesteal works on-kill.
        OnHitEnemy();
        float rhythmScore = GameManager.MusicPlayer.GetRhythmSyncScore();
        if (rhythmScore > 0)
        {
            GameManager.PlayerManager.AddScreenShake(.15f * rhythmScore, .5f);
        }
        else
        {
            GameManager.PlayerManager.AddScreenShake(.05f, .25f);
        }
    }

    public void OnHitEnemy()
    {
        GameManager.PlayerManager.HitScreenZoom();
        playerHealth.currentHealth += lifesteal;
    }

}
