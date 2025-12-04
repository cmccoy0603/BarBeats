using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HitIndicator : MonoBehaviour
{
    private Color perfectColor = Color.red;
    private Color earlyColor = Color.green;
    private Color lateColor = Color.blue;

    [SerializeField] private GameObject indicatorPool;
    private Queue<TextMeshProUGUI> pooledText;
    private List<Tuple<TextMeshProUGUI, float>> usedText;
    private bool _played_recently = false;

    public float effectDuration = .5f;

    private void Awake()
    {
        pooledText = new Queue<TextMeshProUGUI>();
        usedText = new List<Tuple<TextMeshProUGUI, float>>();
        
        for (int i = 0; i < indicatorPool.transform.childCount; i++)
        {
            var child = indicatorPool.transform.GetChild(i);
            var childText = child.GetComponent<TextMeshProUGUI>();
            if (!childText) continue;
            pooledText.Enqueue(childText);
        }
    }

    public void Update()
    {
        usedText.RemoveAll(pair =>
        {
            var (text, startTime) = pair;
            if (Time.time >= startTime + effectDuration)
            {
                text.transform.localPosition = Vector3.zero;
                text.gameObject.SetActive(false);
                pooledText.Enqueue(text);
                return true;
            }
            return false;
        });

        foreach (var pair in usedText)
        {
            pair.Item1.transform.localPosition += Vector3.up * Time.deltaTime;
        }
    }

    public void PlayIndicator(float rhythmScore)
    {
        if (_played_recently) return;
        
        // Get a text element from the list
        if (pooledText.Count <= 0)
        {
            Debug.LogError($"Ran out of indicators in the queue. Look at increasing the size from {indicatorPool.transform.childCount}");
            return;
        }
        
        Debug.Log("Pool count: " + pooledText.Count);

        TextMeshProUGUI text = pooledText.Dequeue();
        
        if (rhythmScore < .8)
        {
            text.text = "EARLY";
            text.color = earlyColor;
        }
        else
        {
            text.text = "PERFECT";
            text.color = perfectColor;
        }
        
        text.gameObject.SetActive(true);
        usedText.Add(new Tuple<TextMeshProUGUI, float>(text, Time.time));
        
        _played_recently = true;
        StartCoroutine(ResetPlayedRecently());
    }

    private IEnumerator ResetPlayedRecently()
    {
        yield return new WaitForSeconds(.1f);
        _played_recently = false;
    }
}
