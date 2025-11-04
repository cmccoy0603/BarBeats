using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class KnockbackFeedback : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;

    [SerializeField] public float strength = 16, delay = 0.15f;

    public UnityEvent OnBegin, OnEnd;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void ApplyKnockback(GameObject source)
    {
        StopAllCoroutines();
        Vector2 knockbackDir = (gameObject.transform.position - source.transform.position).normalized;
        _rigidbody2D.AddForce(knockbackDir * strength, ForceMode2D.Impulse);

        OnBegin?.Invoke();
        StartCoroutine(Reset());
    }

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(delay);
        _rigidbody2D.linearVelocity = Vector2.zero;
        OnEnd?.Invoke();
    }
}
