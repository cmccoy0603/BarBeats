using System;
using System.Collections;
using Enums;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class EnemyToSpawn<TKey, TValue>
{
    public TKey enemyPrefab;
    public TValue spawnFrequency;

    public EnemyToSpawn(TKey enemyPrefab, TValue spawnFrequency)
    {
        this.enemyPrefab = enemyPrefab;
        this.spawnFrequency = spawnFrequency;
    }
}


public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyToSpawn<GameObject, float>[] enemiesToSpawn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        foreach (var spawnRecord in enemiesToSpawn)
        {
            StartCoroutine(spawnEmemies(spawnRecord.spawnFrequency, spawnRecord.enemyPrefab));
        }
    }

    IEnumerator spawnEmemies(float delay, GameObject enemy)
    {
        yield return new WaitForSeconds(delay);
        if (GameManager.GameState == GameState.PLAYING && !GameManager.IsTutorial)
        {
            GameObject newEnemy = Instantiate(enemy, new Vector3(UnityEngine.Random.Range(-5f, 5), Random.Range(-6f, 6), 0),
                Quaternion.identity);
        }
        StartCoroutine(spawnEmemies(delay, enemy));
    }
}
