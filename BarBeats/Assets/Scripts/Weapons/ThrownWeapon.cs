using Unity.VisualScripting;
using UnityEngine;

public class ThrownWeapon : MonoBehaviour
{
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private DropWeapon dropWeapon;
    private WeaponData _weaponData;
    private float _durability;
    private LayerMask _layerMask;
    private bool _stopAfterHit = true;
    private float damageCooldown = 1f;
    private float _lastDamageTime = -Mathf.Infinity;
    
    public void Throw(WeaponData data, float durability, LayerMask mask)
    {
        _weaponData = data;
        _durability = durability;
        _layerMask = mask;
    }
    
    private void OnCollisionEnter2D(Collision2D collision) => TryDamage(collision.gameObject);
    private void OnCollisionStay2D(Collision2D collision)  => TryDamage(collision.gameObject);

    private void TryDamage(GameObject other)
    {
        // If we don't want to collide with this layer
        if (((1 << other.layer) & _layerMask.value) == 0) return;

        if (damageCooldown > 0f && Time.time - _lastDamageTime < damageCooldown) return;

        var health = other.GetComponent<Health>() ?? other.GetComponentInParent<Health>();
        if (health == null)
        {
            return;
        }

        health.TakeDamage(_weaponData.damage, gameObject);
        _lastDamageTime = Time.time;

        if (!_stopAfterHit) return;
        
        if (_durability > 0)
        {
            // drop instead of destroying
            DropWeapon();
        }
        Destroy(gameObject);
    }

    // Now we drop but with some special stuff
    private void DropWeapon()
    {
        dropWeapon.Drop(_weaponData, _durability);
    }
}
