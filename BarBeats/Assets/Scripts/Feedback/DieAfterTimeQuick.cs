using System.Collections;
using UnityEngine;

public class DieAfterTimeQuick : MonoBehaviour
{
    [SerializeField] private float lifeTime;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
