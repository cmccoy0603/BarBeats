using UnityEngine;

public class DroppedWeapon : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private WeaponData _weaponData;
    private float _durability;

    private void OnCollisionEnter2D(Collision2D collision) => TryPickup(collision.gameObject);
    private void OnCollisionStay2D(Collision2D collision)  => TryPickup(collision.gameObject);
    private void OnTriggerEnter2D(Collider2D other)        => TryPickup(other.gameObject);
    private void OnTriggerStay2D(Collider2D other)         => TryPickup(other.gameObject);

    // When this thing is first dropped
    public void Drop(WeaponData data, float durability)
    {
        _weaponData = data;
        _durability = durability;

        if (!_weaponData.weaponSprite) return;

        spriteRenderer.sprite = _weaponData.weaponSprite;
    }

    public void TryPickup(GameObject gameObject)
    {
        // If its not the player return
        if (!gameObject.CompareTag("Player")) return;
        
        // Get the holder component
        WeaponHolder holder = gameObject.GetComponentInChildren<WeaponHolder>();

        // The player doesn't have a weapon holder
        if (!holder) return;

        // They already have a weapon
        if (!holder.CanPickup()) return;
        
        // Finally just give them the weapon
        Pickup(holder);
    }

    // Someone (the player) is trying to pick this up, letsa go
    public void Pickup(WeaponHolder holder)
    {
        // Actually set stuff
        holder.SetWeapon(_weaponData, _durability);
        
        // kys
        Destroy(gameObject);
    }
}
