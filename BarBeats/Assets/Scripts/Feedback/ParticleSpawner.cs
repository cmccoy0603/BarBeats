using System;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> particlesToSpawn;
    [SerializeField] private GameObject hitParticle;

    public void SpawnParticle()
    {
        foreach (GameObject particle in particlesToSpawn)
        {
            GameObject newEnemy = Instantiate(particle, transform.position,
                Quaternion.identity, transform.parent);
        }
    }

    public void SpawnHitParticles(GameObject source)
    {
        // Gives the vector pointing from the source of the hit to the thing hit
        Vector2 vectorOut = (transform.position - source.transform.position).normalized;
        float zRot = Mathf.Atan2(vectorOut.y, vectorOut.x);

        Quaternion q = Quaternion.Euler(0, 0, zRot * Mathf.Rad2Deg);

        GameObject newEnemy = Instantiate(hitParticle, transform.position, q, transform.parent);

    }
}
