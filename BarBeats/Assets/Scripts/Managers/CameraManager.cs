using System;
using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private float baseFov = 5.0f;
    [SerializeField] private float minimumFov = 4.0f;
    [SerializeField] private float resetTime = 10.0f;
    
    private Camera _camera;
    private ScreenShake _screenShake;
    private float _targetFov;
    private float zoomInSpeed = .05f;
    private float zoomOutSpeed = .0005f;

    private void Awake()
    {
        _camera = Camera.main;
        _screenShake = _camera.GetComponent<ScreenShake>();
        _screenShake.enabled = false;
        _targetFov = baseFov;
    }

    private void Update()
    {
        float currentSize = _camera.orthographicSize;
        float zoomSpeed = _targetFov > currentSize
            ? zoomOutSpeed
            : zoomInSpeed;
        
        _camera.orthographicSize = Mathf.Lerp(currentSize, _targetFov, zoomSpeed);
    }

    public void AddScreenShake(float magnitude, float duration)
    {
        _screenShake.enabled = true;
        _screenShake.AddShake(magnitude, duration);
    }

    public void ZoomIn(float changeAmount = .15f)
    {
        _targetFov = Mathf.Clamp(_targetFov - changeAmount, minimumFov, baseFov);
        StopAllCoroutines();
        StartCoroutine(ResetCamera());
    }

    public void ZoomOut()
    {
        _targetFov = baseFov;
    }

    public IEnumerator ResetCamera()
    {
        yield return new WaitForSeconds(resetTime);
        Debug.Log("resetting the camera");
        _targetFov = baseFov;
    }
}
