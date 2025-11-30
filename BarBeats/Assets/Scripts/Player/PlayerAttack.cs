using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows.WebCam;

public class PlayerAttack : MonoBehaviour
{
    // All stuff we get from serialize field
    [SerializeField] private Animator animator;
    public LayerMask enemyLayer;
    public float attackRange = 1;
    public float attackSpread = 1;
    public float weaponDamage = 10;
    public float meleeSpeed = .25f;
    public GameObject weapon;

    private Camera cam;

    // We'll probably want a weapon for the player to have. That way we can switch it out. For now, nah
    private Transform _weaponTransform;
    private Polygon _weaponRender;
    private PolygonCollider2D _polygonCollider2D;
    [SerializeField] private WeaponHolder weaponHolder;

    private Vector2 _widthHeight = Vector2.zero;
    
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
        _mousePositionAction = InputSystem.actions.FindAction("MousePos");
        _joystickPositionAction = InputSystem.actions.FindAction("StickPos");
        _keysPositionAction = InputSystem.actions.FindAction("KeysPos");
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        _widthHeight = spriteRenderer.size / 2;

        _weaponTransform = weapon.transform;

        // Modify the actual hit box
        _polygonCollider2D = weapon.GetComponentInChildren<PolygonCollider2D>();
        Vector2[] polygonPoints = _polygonCollider2D.points;
        polygonPoints[1] = new Vector2(attackRange, attackSpread);
        polygonPoints[2] = new Vector2(attackRange, -attackSpread);
        _polygonCollider2D.points = polygonPoints;

        _weaponRender = weapon.GetComponentInChildren<Polygon>();
        _weaponRender.DrawPolygon(4, polygonPoints);

        weapon.SetActive(false);
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
        if (!weaponHolder.Attack(angle))
        {
            return;
        }
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
        Debug.Log("Hit enemy");
        GameManager.PlayerManager.HitScreenZoom();
    }

}
