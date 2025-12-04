using System.Collections;
using UnityEngine;

public class TutorialAutoProgress : MonoBehaviour
{
    private int childIndex = 1;

    private int maxIndex;

    private GameObject _lastObject;
    
    [SerializeField] private float progressionSpeed = 8.0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxIndex = transform.childCount;
        _lastObject = transform.GetChild(0).gameObject;
        _lastObject.SetActive(true);
        StartCoroutine(Progress());
    }

    private IEnumerator Progress()
    {
        yield return new WaitForSeconds(progressionSpeed);
        
        if (_lastObject)
        {
            _lastObject.SetActive(false);
        }
        _lastObject = transform.GetChild(childIndex).gameObject;
        _lastObject.SetActive(true);
        childIndex += 1;

        if (childIndex != maxIndex)
        {
            StartCoroutine(Progress());
        }
    }
}
