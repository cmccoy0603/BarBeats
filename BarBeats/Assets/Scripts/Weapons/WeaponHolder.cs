using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class WeaponHolder : MonoBehaviour
{
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private WeaponData unarmedWeaponData;  // What weapon data should we use if the weapon holder is unarmed?
    [SerializeField] private PolygonCollider2D weaponCollider;
    [SerializeField] private Transform weaponPivot;
    [SerializeField] private LayerMask hitLayer;

    private bool _isPlayer = false;
    private float _currentDurability;
    private bool _canAttack = true;
    private bool _hasWeapon;
    // The player still can use her fists to punch stuff, so we kind of make a clause for using the unarmed weapon data
    private bool _unarmedWeapon = false;

    public UnityEvent<String> OnWeaponChange;

    private void Start()
    {
        SetWeapon(weaponData, weaponData.durability);
        if (gameObject.CompareTag("Player"))
        {
            _isPlayer = true;
        }
    }
    
    // This weapon class is kind of a holder for different possible weapons
    // so whenever you pick one up, we have to update the weapon
    public void SetWeapon(WeaponData data, float durability)
    {
        weaponData = data;
        _hasWeapon = true;
        _currentDurability = durability;
        
        // Use the weapon data for initial stuff
        SetCollider();
        
        OnWeaponChange?.Invoke(weaponData.weaponName);

        if (data != unarmedWeaponData)
        {
            _unarmedWeapon = false;
        }
    }

    // An agnostic function for someone with a weapon to swing it and hit whatever they're mad at
    public bool Attack(float angle)
    {
        // Check cooldown
        if (!_canAttack || !_hasWeapon)
        {
            return false;
        }
        // Rotate the collider based on the attack angle
        RotateCollider(angle);
        
        // Play effect
        PlayEffect(angle);
        
        // Then check collisions and get the things to hit
        List<Health> thingsToHurt = CheckCollisions();
        
        float rhythmScore = GameManager.MusicPlayer.GetRhythmSyncScore();
        float rhythmScoreMult = rhythmScore >= .8
            ? 1.2f + rhythmScore
            : rhythmScore + .1f;

        float damage = _isPlayer
            ? ((weaponData.damage + GameManager.PlayerManager.PlayerStats.DamageAdd) * GameManager.PlayerManager.PlayerStats.DamageMult) * rhythmScoreMult
            : weaponData.damage;
        
        // Hurt the things
        foreach (Health thing in thingsToHurt)
        {
            thing.TakeDamage(damage, transform.parent.gameObject);
        }
        
        // Handle attack speed
        StartCoroutine(resetAttack(weaponData.fireRate));
        
        // Check durability
        DamageDurability(weaponData.swingDurabilityDec);

        _canAttack = false;
        return true;
    }

    // Sometimes you want to hurl something
    public void Throw(float angle)
    {
        if (!weaponData.canThrow) return;
        // Take durability damage
        DamageDurability(weaponData.thrownDurabilityDec);
        
        // I  guess instantiate something and hurl it
        Quaternion thrownRot = Quaternion.Euler(0, 0, angle);
        Vector2 spawnPos = weaponPivot.position + (new Vector3(MathF.Cos(angle), Mathf.Sin(angle), 0).normalized * weaponData.startHeight);
        GameObject thrownWeapon = Instantiate(weaponData.thrownPrefab, spawnPos, thrownRot);
        
        // Find our nice component
        ThrownWeapon thrownInfo = thrownWeapon.GetComponent<ThrownWeapon>();
        if (thrownInfo)
        {
            thrownInfo.Throw(weaponData, _currentDurability, hitLayer, weaponData.oneShot);
        }
        else
        {
            Debug.LogError("No thrown weapon component on thrown weapon prefab on weapon data: " + weaponData.name);
        }

        // Hurl
        Rigidbody2D weaponsRigidbody = thrownWeapon.GetComponent<Rigidbody2D>();
        if (!weaponsRigidbody) return;
        Vector2 force = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized * weaponData.throwSpeed;
        weaponsRigidbody.AddForceAtPosition(force, Vector2.zero);
        
        if (!weaponData.oneShot) return;    // Some weapons can be thrown a lot (javelin?), some just once. So we think about that... I guess
        
        // TODO: Don't just call break, do a unique thing
        Break();
    }

    void DamageDurability(float amount)
    {
        _currentDurability -= amount;

        if (_currentDurability <= 0)
        {
            // break the weapon
            Break();
        }
    }

    void Break()
    {
        // Unset weapon data
        _hasWeapon = false;
        weaponData = null;

        // Update stuff to reflect that, maybe play a sound
        
        // if player, give the unarmed weapon data
        if (_isPlayer)
        {
            SetWeapon(unarmedWeaponData, 1);
            _unarmedWeapon = true;
        }
    }

    void PlayEffect(float angle)
    {
        float x = weaponData.endHeight;
        float y = weaponData.endWidth;

        Vector3 spawnPos = new Vector3(Mathf.Cos(angle) * x, Mathf.Sin(angle) * y, 0);
        spawnPos += transform.position;
        
        Quaternion rotation = quaternion.Euler(0, 0, angle);
        
        GameObject effect = Instantiate(weaponData.attackEffect, spawnPos, rotation);
        effect.transform.localScale = new Vector3(Mathf.Min(x, 2), y * 2, 1);
    }

    // Updates the current polygon collider points with the current weapon data
    public void SetCollider()
    {
        // Sanity check
        if (!weaponData)
        {
            return;
        }
        
        float heightAdditive = _isPlayer && !_unarmedWeapon
            ? GameManager.PlayerManager.PlayerStats.AttackRangeAdd
            : 0.0f;
        float widthAdditive = _isPlayer && !_unarmedWeapon
            ? GameManager.PlayerManager.PlayerStats.AttackSpreadAdd
            : 0.0f;

        // Update the points in a clockwise order
        Vector2[] points = new Vector2[4];
        points[0] = new Vector2(weaponData.startHeight, weaponData.startWidth + widthAdditive);
        points[1] = new Vector2(weaponData.endHeight + heightAdditive, weaponData.endWidth + widthAdditive);
        points[2] = new Vector2(weaponData.endHeight + heightAdditive, -weaponData.endWidth - widthAdditive);
        points[3] = new Vector2(weaponData.startHeight, -weaponData.startWidth - widthAdditive);
        weaponCollider.pathCount = 1; 

        // Set the points for the first path (index 0)
        weaponCollider.SetPath(0, points); 
    }
    
    // Rotate the collider by a certain amount
    void RotateCollider(float angle)
    {
        // TODO: Have this be based off of the pivot somehow
        Vector2 startPos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        weaponPivot.localPosition = startPos;
        weaponPivot.localRotation = Quaternion.Euler(0,0,angle * Mathf.Rad2Deg);
    }
    
    List<Health> CheckCollisions()
    {
        List<Collider2D> overlappingColliders = new List<Collider2D>();
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(hitLayer);
        contactFilter.useTriggers = false;
        int numColliders = weaponCollider.Overlap(contactFilter, overlappingColliders);
        List<Health> thingsToHit = new List<Health>();

        if (numColliders > 0)
        {
            foreach (Collider2D enemy in overlappingColliders)
            {
                Health enemyHealth = enemy.gameObject.GetComponent<Health>();
                if (!enemyHealth) continue;

                thingsToHit.Add(enemyHealth);
            }
        }

        return thingsToHit;
    }

    private IEnumerator resetAttack(float time)
    {
        yield return new WaitForSeconds(time);

        _canAttack = true;
    }

    public bool HasWeapon()
    {
        return _hasWeapon;
    }

    public bool CanPickup()
    {
        return _unarmedWeapon;
    }

    public WeaponData EquippedWeaponData()
    {
        return weaponData;
    }

    public float CurrentDurability()
    {
        return _currentDurability;
    }

    public bool CanAttack()
    {
        return _canAttack && _hasWeapon;
    }

    public float GetDurabilityRatio()
    {
        return _currentDurability / weaponData.durability;
    }
}
