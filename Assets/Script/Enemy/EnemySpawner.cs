using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPool))]
public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> AllWaypoints = new();
    private ObjectPool _enemyPool;

    [Header("Enemie Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int maxEnemiesToInstantiate;
    [SerializeField, Min(0.0f)] private float spawnRate;

    private int _currentNumberOfEnemies;
    private float _timer;

    private void Awake()
    {
        _enemyPool = gameObject.GetComponent<ObjectPool>();
        _enemyPool.prefab = enemyPrefab;
        _enemyPool.initialPoolSize = maxEnemiesToInstantiate;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= spawnRate && _currentNumberOfEnemies < maxEnemiesToInstantiate)
        {
            _timer = 0.0f;
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        GameObject enemy = _enemyPool.GetObject();
        enemy.transform.position = transform.position;

        EnemyBehaviour enemyBehaviour = enemy.GetComponent<EnemyBehaviour>();
        enemyBehaviour.Spawner = this;

        enemyBehaviour.ResetEnemy();
        enemyBehaviour.IsCreate = true;
        _currentNumberOfEnemies++;
    }

    public void ReturnEnemyToPool(GameObject enemy)
    {
        _enemyPool.ReturnObject(enemy);
        _currentNumberOfEnemies--;
    }

    public GameObject GetNextWaypoint(int index)
    {
        if (index < AllWaypoints.Count)
        {
            return AllWaypoints[index];
        }

        return AllWaypoints[AllWaypoints.Count - 1];
    }
}