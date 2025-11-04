using System;
using System.Collections;
using UnityEngine;

public class FlashFeedback : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Material _spriteDefaultMat;

    [SerializeField] public float delay = .15f;
    [SerializeField] public Material flashMaterial;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteDefaultMat = _spriteRenderer.material;
    }

    public void ApplyFlash()
    {
        StopAllCoroutines();
        _spriteRenderer.material = flashMaterial;
        StartCoroutine(StopFlash());
    }

    IEnumerator StopFlash()
    {
        yield return new WaitForSeconds(delay);
        _spriteRenderer.material = _spriteDefaultMat;
    }
}
