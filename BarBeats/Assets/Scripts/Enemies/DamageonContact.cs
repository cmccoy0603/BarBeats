// DamageOnContact2D.cs
using UnityEngine;

public class DamageOnContact2D : MonoBehaviour
{
    public float damage = 10f;
    public float damageCooldown = 1f;
    public bool destroySelfAfterHit = false;

    private float lastDamageTime = -Mathf.Infinity;

    private void OnCollisionEnter2D(Collision2D collision) => TryDamage(collision.gameObject);
    private void OnCollisionStay2D(Collision2D collision)  => TryDamage(collision.gameObject);
    private void OnTriggerEnter2D(Collider2D other)       => TryDamage(other.gameObject);
    private void OnTriggerStay2D(Collider2D other)        => TryDamage(other.gameObject);

    private void TryDamage(GameObject other)
    {
        if (!other.CompareTag("Player")) return;

        if (damageCooldown > 0f && Time.time - lastDamageTime < damageCooldown) return;

        var health = other.GetComponent<Health>() ?? other.GetComponentInParent<Health>();
        if (health == null)
        {
            Debug.LogWarning($"DamageOnContact2D: hit object tagged 'Player' ({other.name}) has no Health component.");
            return;
        }

        health.TakeDamage(damage);
        lastDamageTime = Time.time;

        if (destroySelfAfterHit)
            Destroy(gameObject);
    }
}
