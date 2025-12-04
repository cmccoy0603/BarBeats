using System.Collections;
using UnityEngine;

public class IframeFeedback : MonoBehaviour
{
    private Collider2D _collider2D;
    private SpriteRenderer _sr;

    public float IframeDuration = .25f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _collider2D = GetComponent<Collider2D>();
        _sr = GetComponent<SpriteRenderer>();
    }
    
    public void StartIframes()
    {
        StartCoroutine(Iframes());
    }

    public IEnumerator Iframes()
    {
        _collider2D.enabled = false;
        Color preColor = _sr.color;
        // We can also make the alpha?
        Color invulnColor = new Color(preColor.r, preColor.g, preColor.b, .25f);
        _sr.color = invulnColor;
        
        yield return new WaitForSeconds(IframeDuration);
        _collider2D.enabled = true;
        _sr.color = preColor;
    }
}
