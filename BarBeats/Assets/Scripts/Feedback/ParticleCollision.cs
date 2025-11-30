using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ParticleCollision : MonoBehaviour
{
    [SerializeField] private GameObject splatPrefab;
    [SerializeField] private Transform splatHolder;
    [SerializeField] private AK.Wwise.Event collisionSound;
    [SerializeField] private float soundCapResetSpeed = 0.55f;
    [SerializeField] private int maxSounds = 3;
    
    private ParticleSystem _particle;
    private List<ParticleCollisionEvent> _collisionEvents = new List<ParticleCollisionEvent>();
    private int _soundsPlayed;
    private bool _startedSound = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the particle system this component is attached to
        _particle = GetComponent<ParticleSystem>();
    }

    private void OnParticleCollision(GameObject other)
    {
        // Get collision events from particle
        ParticlePhysicsExtensions.GetCollisionEvents(_particle, other, _collisionEvents);
        splatHolder = other.transform;

        foreach (var particleCollision in _collisionEvents)
        {
            // Make the splat prefab
            Instantiate(splatPrefab, particleCollision.intersection,
                Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)), splatHolder);

            if (_soundsPlayed < maxSounds)
            {
                _soundsPlayed++;
                // TODO: Put in sound playing logic
            }
            else
            {
                _startedSound = true;
                StartCoroutine(ResetSound());
            }
        }
    }

    private IEnumerator ResetSound()
    {
        yield return new WaitForSeconds(soundCapResetSpeed);
        _soundsPlayed = 0;
        _startedSound = false;
    }
}
