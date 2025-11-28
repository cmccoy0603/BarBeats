using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private PolygonCollider2D weaponCollider;
    [SerializeField] private Transform weaponPivot;
    [SerializeField] private LayerMask hitLayer;

    private float currentDurability;
    private bool _canAttack = true;

    private void Start()
    {
        SetWeapon(weaponData);
    }


    // This weapon class is kind of a holder for different possible weapons
    // so whenever you pick one up, we have to update the weapon
    public void SetWeapon(WeaponData data)
    {
        weaponData = data;
        
        // Use the weapon data for initial stuff
        SetCollider();
    }

    // An agnostic function for someone with a weapon to swing it and hit whatever they're mad at
    public bool Attack(float angle)
    {
        // Check cooldown
        if (!_canAttack)
        {
            return false;
        }
        // Rotate the collider based on the attack angle
        RotateCollider(angle);
        
        // Then check collisions and get the things to hit
        List<Health> thingsToHurt = CheckCollisions();
        
        // Hurt the things
        foreach (Health thing in thingsToHurt)
        {
            thing.TakeDamage(weaponData.damage, gameObject);
        }
        
        // Check durability
        DamageDurability(weaponData.swingDurabilityDec);
        
        // Handle attack speed
        StartCoroutine(resetAttack(weaponData.fireRate));
        _canAttack = false;
        return true;
    }

    // Sometimes you want to hurl something
    public void Throw(float angle)
    {
        // I  guess instantiate something and hurl it
        GameObject thrownWeapon = Instantiate(weaponData.thrownPrefab, weaponPivot.position, Quaternion.identity);
        
        // Take durability damage
        DamageDurability(weaponData.thrownDurabilityDec);
        
        // Hurl
        Rigidbody2D weaponsRigidbody = thrownWeapon.GetComponent<Rigidbody2D>();
        if (!weaponsRigidbody) return;
        Vector2 force = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * weaponData.throwSpeed;
        weaponsRigidbody.AddForceAtPosition(force, Vector2.zero);
    }

    void DamageDurability(float amount)
    {
        currentDurability -= amount;

        if (currentDurability <= 0)
        {
            // break the weapon
            Break();
        }
    }

    void Break()
    {
        // Unset weapon data
        
        // Update stuff to reflect that
    }

    // Updates the current polygon collider points with the current weapon data
    void SetCollider()
    {
        // Update the points in a clockwise order
        Vector2[] points = new Vector2[4];
        Debug.Log(weaponData.startWidth);
        points[0] = new Vector2(weaponData.startHeight, weaponData.startWidth);
        points[1] = new Vector2(weaponData.endHeight, weaponData.endWidth);
        points[2] = new Vector2(weaponData.endHeight, -weaponData.endWidth);
        points[3] = new Vector2(weaponData.startHeight, -weaponData.startWidth);
        Debug.Log(points[0]);
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
}
