using System;
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
        
        // Use the weapon date for initial stuff
        
        
    }

    // An agnostic function for someone with a weapon to swing it and hit whatever they're mad at
    public void Attack(float angle)
    {
        // First we want to modify the collision for the weapon based off of the weapon data
        
        // Then check collisions and get the things to hit
        
        // Hurt the things
        
        // Check durability
        
        // Maybe break
    }

    // Sometimes you want to hurl something
    public void Throw(float angle)
    {
        // I  guess instantiate something and hurl it
    }

    // Set the points for the polygon collider to check for collisions based on the weapon data
    void ModifyCollider()
    {
        // Get the pivot location
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
