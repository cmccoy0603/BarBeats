using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI latencyLabel;
    [SerializeField] private Slider latencySlider;

    private void Start()
    {
        var latency = AkEventList.audio_device_latency_ms;
        latencyLabel.text = latency.ToString();
        //latencySlider.value = latency;
        //latencySlider.minValue = latency - 20;
        //latencySlider.maxValue = latency - 20;
    }

    public void ValueChange(float newAmount)
    {
        int latency = Mathf.FloorToInt(newAmount);
        AkEventList.audio_device_latency_ms = Mathf.FloorToInt(latency);
        latencyLabel.text = latency.ToString();
    }
}
