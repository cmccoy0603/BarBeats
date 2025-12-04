using System;
using UnityEngine;

public class BobAnimation : MonoBehaviour
{
    [SerializeField] private float frequency = 10.0f;

    [SerializeField] private float amplitude = .1f;

    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        float scaleAmount = originalScale.y + (Mathf.Sin(Time.time * frequency) * amplitude);
        transform.localScale = new Vector3(originalScale.x, scaleAmount, originalScale.z);
    }
}
