using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    //Setting up variables to control the screen shake
    private float _magnitude = 0;
    private float _duration = 0;
    private float _fadeTime;
    private float _shakeRotation;

    public float ShakeMultiplier = 7f;
        
    //Center of shake
    private Vector3 _cameraTransform;

    private void OnEnable()
    {
        _cameraTransform = transform.localPosition;
    }

    private void Update()
    {
        transform.localPosition = _cameraTransform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //If you should shake the screen
        if (_duration > 0)
        {
            float xAmount = Random.Range(-1, 1) * _magnitude;
            float yAmount = Random.Range(-1, 1) * _magnitude;
            transform.localPosition += new Vector3(xAmount, yAmount, 0);
   
            _duration -= Time.deltaTime;

            _magnitude = Mathf.MoveTowards(_magnitude, 0f, _fadeTime * Time.deltaTime);

            _shakeRotation = Mathf.MoveTowards(_shakeRotation, 0f, _fadeTime * ShakeMultiplier * Time.deltaTime);
        }
        else
        {
            transform.localPosition = _cameraTransform;
            _duration = 0f;
            enabled = false;
        }
        transform.rotation = Quaternion.Euler(0, 0, _shakeRotation * Random.Range(-1, 1));
    }

    // Function to add screen shake 
    // Can be called whenever and it'll take over the previously applied screen shake
    // Magnitude: 1 is a lot of screen shake, .1 for a reasonable amount
    public void AddShake(float magnitude, float duration)
    {
        _duration = duration;
        _magnitude = magnitude;
        _fadeTime = _magnitude / _duration;

        _shakeRotation = _magnitude * ShakeMultiplier;
    }
}
