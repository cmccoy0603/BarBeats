using System.Collections.Generic;
using UnityEngine;

public class RandomSpriteSelector : MonoBehaviour
{
    [SerializeField] private List<Sprite> possibleSprites;

    [SerializeField] private SpriteRenderer spriteRenderer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int randomIndex = Random.Range(0, possibleSprites.Count);
        Sprite randomFromList = possibleSprites[randomIndex];
        spriteRenderer.sprite = randomFromList;
    }
}
