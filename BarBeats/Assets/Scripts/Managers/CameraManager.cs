using System;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Camera _camera;
    private ScreenShake _screenShake;

    private void Awake()
    {
        _camera = Camera.main;
        _screenShake = _camera.GetComponent<ScreenShake>();
        _screenShake.enabled = false;
    }

    public void AddScreenShake(float magnitude, float duration)
    {
        _screenShake.enabled = true;
        _screenShake.AddShake(magnitude, duration);
    }
}
