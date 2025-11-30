using UnityEngine;

public class SetColliderFromSprite : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private BoxCollider2D collider2D;
    
    private Vector2 _widthHeight;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _widthHeight = spriteRenderer.size;
        collider2D.size = _widthHeight;
    }
}
