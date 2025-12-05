using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float currentHealth = 100f;
    private bool destroyGameObjectOnDeath = true;
    private float deathDestroyDelay = 0f;
    private bool invulnerable = false;
    private OnHit hitFeedback = null;

    public UnityEvent OnDeath;
    public UnityEvent<GameObject> OnHit;
    public UnityEvent OnHealthChange;

    [SerializeField]
    private float maxHealth;
    public bool IsDead => currentHealth <= 0f;

    private void Awake()
    {
        currentHealth = maxHealth;
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

        if (source)
        {
            // See if the source has a schmancy component
            hitFeedback = source.GetComponent<OnHit>();
        }

        // Negative damage will heal
        currentHealth -= amount;
        OnHealthChange?.Invoke();
        if (currentHealth <= 0f)
        {
            Die();
            if (hitFeedback)
            {
                hitFeedback.OnKillEnemy(gameObject);
            }
            return true;
        }

        if (source != null)
        {
            OnHit?.Invoke(source);
        }

        if (hitFeedback)
        {
            hitFeedback.OnHitEnemy(gameObject);
        }

        hitFeedback = null;

        return false;
    }

    public void IncreaseMaxHealth(float amount)
    {
        maxHealth += amount;
        currentHealth += amount;
        OnHealthChange?.Invoke();
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        OnHealthChange?.Invoke();
    }

    private void Die(GameObject source = null)
    {
        // Fire any inspector-assigned listeners
        OnDeath?.Invoke();

        // If the player dies we don't want to just delete them, we instead want to do some other stuff first
        if (gameObject.CompareTag("Player"))
        {
            GameManager.PlayerManager.PlayerDeath();
            return;
        }

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

    public float GetMaxHealth()
    {
        return maxHealth;
    }
}
