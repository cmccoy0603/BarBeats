using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public LayerMask enemyLayer;
    public float attackRange = 1;
    public float attackSpread = 1;
    public float weaponDamage = 20;
    public GameObject weapon;

    private Camera cam;
    // We'll probably want a weapon for the player to have. That way we can switch it out. For now, nah
    private InputAction _positionAction;
    private Transform _weaponTransform;
    private Polygon _weaponRender;
    private PolygonCollider2D _polygonCollider2D;

    private Vector2 _widthHeight = Vector2.zero;

    private void Awake()
    {
        cam = Camera.main;
        _positionAction = InputSystem.actions.FindAction("PointerPosition");
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

    public void Attack(InputAction.CallbackContext context)
    {
        if (weapon.activeSelf)
        {
            return;
        }
        weapon.SetActive(true);
        float rhythmScore = GameManager.MusicPlayer.GetRhythmSyncScore();
        Debug.Log("rhythmScore: " + rhythmScore);

        // TODO: Play animation depending on the rhythm score

        Vector3 inputPosition = _positionAction.ReadValue<Vector2>();

        Vector2 aimVector = inputPosition;
        // Our math changes slightly if the player is using a mouse vs a keyboard. We can probably abstract this out in the future
        if (context.control.shortDisplayName == "LMB")
        {
            inputPosition.z = cam.nearClipPlane;
            Vector3 point = cam.ScreenToWorldPoint(inputPosition);

            // Now we get the vector from the player to the point (aim direction)
            aimVector = point - transform.position;
        }

        aimVector.Normalize();

        // Angle in rad
        float angle = Mathf.Atan2(aimVector.y, aimVector.x);
        Vector2 startPos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * _widthHeight;

        _weaponTransform.localPosition = startPos;
        _weaponTransform.localRotation = Quaternion.Euler(0, 0, angle);

        _weaponRender.DrawPolygon(4, _polygonCollider2D.points, angle, rhythmScore);

        // Get collisions?
        float damageMult = rhythmScore > .9 ? 2 : rhythmScore;
        CheckCollisions(damageMult);

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

    IEnumerator StopWeaponAnimation()
    {
        yield return new WaitForSeconds(.25f);
        weapon.SetActive(false);
    }

}
