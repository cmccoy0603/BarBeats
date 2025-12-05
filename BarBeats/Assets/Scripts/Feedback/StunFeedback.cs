using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class StunFeedback : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;

    [SerializeField] public float delay = 0.15f;

    public UnityEvent OnBegin, OnEnd;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void ApplyStun()
    {
        StopAllCoroutines();
        _rigidbody2D.linearVelocity = Vector2.zero;
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
