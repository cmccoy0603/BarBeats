using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private PolygonCollider2D weaponCollider;
    [SerializeField] private Transform weaponPivot;
    [SerializeField] private LayerMask hitLayer;

    private float currentDurability;

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
    }

    // An agnostic function for someone with a weapon to swing it and hit whatever they're mad at
    public void Attack(float angle)
    {
        // First we want to modify the collision for the weapon based off of the weapon data
        ModifyCollider(angle);
        
        // Then check collisions and get the things to hit
        List<Health> thingsToHurt = CheckCollisions();
        
        // Hurt the things
        foreach (Health thing in thingsToHurt)
        {
            thing.TakeDamage(weaponData.damage);
        }
        
        // Check durability
        DamageDurability(weaponData.swingDurabilityDec);
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
    
    // Set the points for the polygon collider to check for collisions based on the weapon data
    void ModifyCollider(float angle)
    {
        // Get the angles for the left and right start of the polygon
        float leftAngle = angle - Mathf.PI / 2.0f;
        float rightAngle = angle + Mathf.PI / 2.0f;
        Vector2 leftVector = new Vector2(Mathf.Cos(leftAngle), Mathf.Sin(leftAngle));
        Vector2 rightVector = new Vector2(Mathf.Cos(rightAngle), Mathf.Sin(rightAngle));
        
        // Update the points in a clockwise order
        Vector2[] points = weaponCollider.points;
        points[0] = leftVector * weaponData.startWidth;
        points[1] = (leftVector * weaponData.endWidth) + (leftVector * weaponData.endHeight);
        points[2] = (rightVector * weaponData.endWidth) + (rightVector * weaponData.endHeight);
        points[3] = rightVector * weaponData.startWidth;
        weaponCollider.points = points;
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

}
