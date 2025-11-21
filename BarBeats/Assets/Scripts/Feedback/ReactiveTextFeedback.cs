using System;
using TMPro;
using UnityEngine;

public class ReactiveTextFeedback : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float scaleAmount = 8;

    private float startScale;

    private void Awake()
    {
        startScale = text.fontSize;
    }

    public void GrowText()
    {
        if (!text) return;
        
        text.fontSize = startScale + scaleAmount;
    }

    public void ShrinkText()
    {
        if (!text) return;

        text.fontSize = startScale;
    }
}
