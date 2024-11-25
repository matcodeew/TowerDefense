using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPool))]
public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnData
    {
        public S_Enemy Enemy = null;
        public int MaxToInstantiate = 0;
        public float SpawnRate;
    }
    public static EnemySpawner Instance;

    [Header("Waypoints")]
    public List<GameObject> AllWaypoints = new();

    [Header("Enemy Settings")]
    [SerializeField] private EnemySpawnData SpawnData;

    private float _timer;
    private ObjectPool _enemyPool;
    private WaveManager _waveManager;

    private void Awake()
    {
        EventsManager.OnWaveStart += UpdateParameter;

        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        _waveManager = WaveManager.Instance;
        _enemyPool = GetComponent<ObjectPool>();
    }
    public void UpdateParameter(S_Enemy enemy, float quantity)
    {
        SpawnData.Enemy = enemy;
        SpawnData.MaxToInstantiate = Mathf.FloorToInt(quantity);
    }

    private void Update()
    {
        if (!_waveManager.CanStartFirstWave) { return; }

        _timer += Time.deltaTime;

        if (_timer >= SpawnData.SpawnRate && _waveManager.CurrentEnemyOnMap < SpawnData.MaxToInstantiate)
        {
            _timer = 0.0f;
            SpawnEnemy(SpawnData.Enemy);
        }
    }

    private void SpawnEnemy(S_Enemy enemyToSpawn)
    {
        GameObject enemy = null;
        switch (enemyToSpawn.type)
        {
            case EnemyType.Normal:
            case EnemyType.Elite:
            case EnemyType.Boss:
                enemy = _enemyPool.GetObject(enemyToSpawn.type);
                break;

            default:
                Debug.LogError($"Unrecognized enemy type: {enemyToSpawn.type}");
                return;
        }

        enemy.transform.position = transform.position;

        EnemyBehaviour enemyBehaviour = enemy.GetComponent<EnemyBehaviour>();
        enemyBehaviour.Spawner = this;
        enemyBehaviour.EnemyData = enemyToSpawn;
        enemyBehaviour.ResetEnemy();
        enemyBehaviour.IsCreate = true;
        _enemyPool.UpdateCurrentLife(enemyBehaviour);

        _waveManager.CurrentEnemyOnMap++;
    }
    public void ReturnEnemyToPool(GameObject enemy, EnemyType type)
    {
        _enemyPool.ReturnObject(enemy, type);
        WaveManager.Instance.EnemyKill++;
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
