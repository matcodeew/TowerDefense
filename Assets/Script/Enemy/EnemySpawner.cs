using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPool))]
public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public struct EnemySpawnData
    {
        public S_Enemy Enemy;
        public int MaxToInstantiate;
        public float SpawnRate;
    }

    [Header("Waypoints")]
    public List<GameObject> AllWaypoints = new();

    [Header("Enemie Settings")]
    [SerializeField] private EnemySpawnData SpawnData; //mettre a jours en fonction de la wave 

    private int _currentNumberOfEnemies;
    private float _timer;
    private ObjectPool _enemyPool;
    private void Awake()
    {
        _enemyPool = gameObject.GetComponent<ObjectPool>();
        _enemyPool.prefab = SpawnData.Enemy.Prefab;
        _enemyPool.initialPoolSize = SpawnData.MaxToInstantiate;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= SpawnData.SpawnRate && _currentNumberOfEnemies < SpawnData.MaxToInstantiate)
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
        enemyBehaviour.EnemyData = SpawnData.Enemy;
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