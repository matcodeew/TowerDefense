using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    #region Struct
    [System.Serializable]
    public struct EnemyParentPool
    {
        public PoolEnemy Normal;
        public PoolEnemy Elite;
        public PoolEnemy Boss;
    }
    [System.Serializable]
    public struct PoolEnemy
    {
        public GameObject Parent;
        public int InitialPoolSize;
        public GameObject Prefab;
        public EnemyType Type;
    }
    #endregion

    [Header("Enemy parent on Hierarchy")]
    [SerializeField] private EnemyParentPool EnemyParent;
    [SerializeField] private List<S_Enemy> AllType;

    public Dictionary<EnemyType, List<GameObject>> pools = new Dictionary<EnemyType, List<GameObject>>();

    private S_Enemy enemyToUpgrade;

    private void Awake()
    {
        EventsManager.OnWaveStart += UpdateStat;

        foreach (var field in typeof(EnemyParentPool).GetFields())
        {
            PoolEnemy enemy = (PoolEnemy)field.GetValue(EnemyParent);
            InitializePool(enemy);
        }
    }
    private void UpdateStat(S_Enemy enemy, int quantity)
    {
        int wave = 0;
        foreach (var enemyType in pools[enemy.type])
        {
            EnemyBehaviour current = enemyType.GetComponent<EnemyBehaviour>();

            switch (enemy.type)
            {
                case EnemyType.Normal:
                    enemyToUpgrade = WaveManager.Instance.typeEnemyToSpawn.Normal;
                    wave = RessourceManager.CurrentWave;
                    break;

                case EnemyType.Elite:
                    enemyToUpgrade = WaveManager.Instance.typeEnemyToSpawn.Elite;
                    wave = RessourceManager.CurrentWave / 4;
                    break;

                case EnemyType.Boss:
                    enemyToUpgrade = WaveManager.Instance.typeEnemyToSpawn.Boss;
                    wave = RessourceManager.CurrentWave / 10;
                    break;

                default:
                    enemyToUpgrade = null;
                    wave = -1;
                    break;
            }


            if (wave != 1)
            {
                current.stat.MaxLife = enemyToUpgrade.MaxLife * (enemyToUpgrade.MaxLifeMultiplicator == 1 ? 1 : (wave * enemyToUpgrade.MaxLifeMultiplicator));
                current.stat.MoveSpeed += enemyToUpgrade.MoveSpeedMultiplicator;
                current.stat.Damage = enemyToUpgrade.Damage * (enemyToUpgrade.DamageMultiplicator == 1 ? 1 : (wave * enemyToUpgrade.DamageMultiplicator));
                UpdateCurrentLife(current);
            }
            else
            {
                current.stat.MaxLife = enemyToUpgrade.MaxLife;
                current.stat.MoveSpeed = enemyToUpgrade.MoveSpeed;
                current.stat.Damage = enemyToUpgrade.Damage;
                UpdateCurrentLife(current);
            }

        }
        float baseSpawnRate = EnemySpawner.Instance.SpawnData.SpawnRate;
        EnemySpawner.Instance.SpawnData.SpawnRate = Mathf.Max(0.1f, baseSpawnRate - 0.005f);
    }

    public void UpdateCurrentLife(EnemyBehaviour current)
    {
        current.stat.CurrentLife = current.stat.MaxLife;
    }
    private void InitializePool(PoolEnemy poolData)
    {
        if (poolData.Prefab == null || poolData.Parent == null)
        {
            return;
        }

        if (!pools.ContainsKey(poolData.Type))
        {
            pools[poolData.Type] = new List<GameObject>();
        }

        for (int i = 0; i < poolData.InitialPoolSize; i++)
        {
            GameObject newEnemy = Instantiate(poolData.Prefab);
            newEnemy.name = $"{poolData.Type} ({i})";
            newEnemy.transform.parent = poolData.Parent.transform;
            newEnemy.SetActive(false);
            pools[poolData.Type].Add(newEnemy);
        }
    }

    public GameObject GetObject(EnemyType type)
    {
        if (!pools.ContainsKey(type))
        {
            return null;
        }

        foreach (GameObject enemy in pools[type])
        {
            if (!enemy.activeInHierarchy)
            {
                enemy.SetActive(true);
                return enemy;
            }
        }

        PoolEnemy poolData = GetPoolData(type);
        if (poolData.Prefab is not null)
        {
            GameObject newEnemy = Instantiate(poolData.Prefab);
            newEnemy.transform.parent = poolData.Parent.transform;
            pools[type].Add(newEnemy);

            newEnemy.SetActive(true);
            return newEnemy;
        }
        return null;
    }

    private IEnumerator ActivateEnemyWithDelay(GameObject enemy, float delay)
    {
        enemy.SetActive(false);
        yield return new WaitForSeconds(delay);
        enemy.SetActive(true);
    }


    public void ReturnObject(GameObject enemy, EnemyType type)
    {
        if (!pools.ContainsKey(type))
        {
            return;
        }
        enemy.SetActive(false);
        enemy.transform.position = Vector3.zero;
    }

    private PoolEnemy GetPoolData(EnemyType type)
    {
        return type switch
        {
            EnemyType.Normal => EnemyParent.Normal,
            EnemyType.Elite => EnemyParent.Elite,
            EnemyType.Boss => EnemyParent.Boss,
            _ => default,
            //  _ = wilcard = tout ce qui n'est pas deja indiquer = return default
        };
    }
}
