using System.Collections.Generic;
using UnityEngine;
using static ObjectPool;

public class ObjectPool : MonoBehaviour
{
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
    [Header("Enemy parent on Hierarchy")]
    [SerializeField] private EnemyParentPool EnemyParent;
    private Dictionary<EnemyType, List<GameObject>> pools = new Dictionary<EnemyType, List<GameObject>>();

    private void Start()
    {
        // Initialize pools for each type of enemy
        InitializePool(EnemyParent.Normal);
        InitializePool(EnemyParent.Elite);
        InitializePool(EnemyParent.Boss);
    }

    private void InitializePool(PoolEnemy poolData)
    {
        if (poolData.Prefab == null || poolData.Parent == null)
        {
            Debug.LogWarning($"Pool for {poolData.Type} is missing required data.");
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
            Debug.LogError($"No pool found for enemy type {type}.");
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

        // If no inactive object is available, instantiate a new one
        PoolEnemy poolData = GetPoolData(type);
        if (poolData.Prefab != null)
        {
            GameObject newEnemy = Instantiate(poolData.Prefab);
            newEnemy.transform.parent = poolData.Parent.transform;
            newEnemy.SetActive(true);
            pools[type].Add(newEnemy);
            return newEnemy;
        }

        Debug.LogError($"Unable to instantiate new enemy for type {type}. Missing prefab or parent.");
        return null;
    }

    public void ReturnObject(GameObject enemy, EnemyType type)
    {
        if (!pools.ContainsKey(type))
        {
            Debug.LogError($"No pool found for enemy type {type}. Cannot return object.");
            return;
        }

        enemy.SetActive(false);
    }

    private PoolEnemy GetPoolData(EnemyType type)
    {
        return type switch
        {
            EnemyType.Normal => EnemyParent.Normal,
            EnemyType.Elite => EnemyParent.Elite,
            EnemyType.Boss => EnemyParent.Boss,
            _ => default,
        };
    }
}
