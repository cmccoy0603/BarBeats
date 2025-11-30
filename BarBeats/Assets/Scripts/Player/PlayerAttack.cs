using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
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
    public float lifesteal = 0f;
    public GameObject weapon;   

    private Camera cam;
    private Health playerHealth;

    // We'll probably want a weapon for the player to have. That way we can switch it out. For now, nah
    private Transform _weaponTransform;
    private Polygon _weaponRender;
    private PolygonCollider2D _polygonCollider2D;

    private Vector2 _widthHeight = Vector2.zero;

    // All of our silly input stuff
    private InputAction _mousePositionAction;
    private InputAction _joystickPositionAction;
    private InputAction _keysPositionAction;
    private InputDevice _attackDevice;
    private bool _attackedThisFrame = false;


    private void Awake()
    {
        cam = Camera.main;
        playerHealth = GetComponent<Health>();
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

        Attack(_attackDevice);
    }

    public void PreAttack(InputAction.CallbackContext context)
    {
        _attackDevice = context.control.device;
        _attackedThisFrame = true;
    }

    public void Attack(InputDevice device)
    {
        _attackedThisFrame = false;
        if (weapon.activeSelf || GameManager.GameState != GameState.PLAYING)
        {
            return;
        }
        weapon.SetActive(true);
        animator.SetTrigger("Attack");

        float rhythmScore = GameManager.MusicPlayer.GetRhythmSyncScore();

        Vector2 aimVector = GetAttackDirection(device);

        aimVector.Normalize();

        // Angle in rad
        float angle = Mathf.Atan2(aimVector.y, aimVector.x);
        Vector2 startPos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * _widthHeight;

        _weaponTransform.localPosition = startPos;
        _weaponTransform.localRotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);

        _weaponRender.DrawPolygon(4, _polygonCollider2D.points, rhythmScore);

        // Get collisions?
        float damageMult = rhythmScore > .9 ? 2 : 1 + (rhythmScore);
        CheckCollisions(damageMult);

        // Player follow through
        GameManager.PlayerManager.MovePlayer(aimVector);

        StartCoroutine(StopWeaponAnimation());

    }

    void CheckCollisions(float damageMult)
    {
        List<Collider2D> overlappingColliders = new List<Collider2D>();
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(enemyLayer);
        contactFilter.useTriggers = false;
        int numColliders = _polygonCollider2D.Overlap(contactFilter, overlappingColliders);

        if (numColliders > 0)
        {
            foreach (Collider2D enemy in overlappingColliders)
            {
                Health enemyHealth = enemy.gameObject.GetComponent<Health>();
                if (!enemyHealth)
                {
                    return;
                }

                enemyHealth.TakeDamage(weaponDamage * damageMult, gameObject);
            }
        }
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

    IEnumerator StopWeaponAnimation()
    {
        yield return new WaitForSeconds(meleeSpeed);
        weapon.SetActive(false);
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
        playerHealth.currentHealth += lifesteal;
    }

}
