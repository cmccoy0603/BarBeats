using System;
using System.Collections;
using UnityEngine;

public class DieAfterTime : MonoBehaviour
{
    [SerializeField] private float lifeTime;

    private float _startTime;

    private SpriteRenderer _sprite;
    private SpriteMask _mask;
    private Color _color;
    private bool _isdying = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _mask = GetComponent<SpriteMask>();
        _sprite = GetComponent<SpriteRenderer>();
        if (_sprite)
        {
            _color = _sprite.color;
        }

        _startTime = Time.time;
    }

    private void Update()
    {
        if (Time.time - _startTime > lifeTime && !_isdying)
        {
            _isdying = true;
            StartCoroutine(Fade());
        }
    }

    private IEnumerator Fade()
    {
        float value = 1;
        
        while (value > 0)
        {
            value -= Time.deltaTime * 0.05f;
            if (_sprite)
            {
                _sprite.color = new Color(_color.r, _color.g, _color.b, value);
            }

            transform.localScale = new Vector3(value, value, value);
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }
}
