using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    private float maxHealth = 100f;
    private bool destroyGameObjectOnDeath = true;
    private float deathDestroyDelay = 0f;
    private bool invulnerable = false;

    public UnityEvent OnDeath;
    public UnityEvent<GameObject> OnHit;

    public float CurrentHealth;
    public bool IsDead => CurrentHealth <= 0f;

    private void Awake()
    {
        CurrentHealth = maxHealth;
    }

    private void OnValidate()
    {
        // Keep sensible values in the Inspector
        maxHealth = Mathf.Max(0.01f, maxHealth);
        deathDestroyDelay = Mathf.Max(0f, deathDestroyDelay);
    }

    /// Returns true if this call caused the object to die.
    public bool TakeDamage(float amount, GameObject source = null)
    {
        if (IsDead || invulnerable)
        {
            return false; // already dead or cant be hit, ignore
        }

        // Negative damage will heal
        CurrentHealth -= amount;

        if (CurrentHealth <= 0f)
        {
            Die();
            return true;
        }

        if (source != null)
        {
            OnHit?.Invoke(source);
        }

        return false;
    }

    private void Die()
    {
        // Fire any inspector-assigned listeners
        OnDeath?.Invoke();

        Destroy(gameObject, deathDestroyDelay);
    }

    public void SetInvulnerable()
    {
        invulnerable = true;
    }

    public void SetVulnerable()
    {
        invulnerable = false;
    }
}
